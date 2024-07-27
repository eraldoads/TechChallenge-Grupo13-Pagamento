using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;

namespace Data.Messaging
{
    public class PagamentoMessageQueue : IPagamentoMessageQueue, IDisposable
    {
        private readonly ILogger<PagamentoMessageQueue> _logger;
        private IConnection _connection;
        private IModel _channel;
        private bool _disposed = false;

        private readonly string _hostname = Environment.GetEnvironmentVariable("RABBIT_HOSTNAME");
        private readonly string _username = Environment.GetEnvironmentVariable("RABBIT_USERNAME");
        private readonly string _password = Environment.GetEnvironmentVariable("RABBIT_PASSWORD");
        private readonly IPagamentoMessageSender _sender;

        private ConcurrentDictionary<string, int> _retryCountDictionary = new ConcurrentDictionary<string, int>();

        public event Func<string, Task> MessageReceived;

        public PagamentoMessageQueue(ILogger<PagamentoMessageQueue> logger, IPagamentoMessageSender sender)
        {
            _logger = logger;
            _sender = sender;
            ConnectRabbitMQ();
        }

        public async Task StartListening()
        {
            EnsureNotDisposed();

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                string content = Encoding.UTF8.GetString(ea.Body.ToArray());

                try
                {
                    if (MessageReceived != null)
                    {
                        await MessageReceived(content);
                    }
                    // Ack se a mensagem foi processada com sucesso
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar mensagem, tentando recolocar na fila...");

                    if (_retryCountDictionary.TryGetValue(content, out int retryCount))
                    {
                        if (retryCount < 3) // Máximo de tentativas
                        {
                            _retryCountDictionary.AddOrUpdate(content, 1, (key, oldValue) => oldValue + 1);
                            // Rejeita e reenfileira a mensagem
                            _channel.BasicNack(ea.DeliveryTag, false, true);
                        }
                        else
                        {
                            _logger.LogError($"Falha ao processar a mensagem após {retryCount} tentativas, descartando a mensagem.");
                            _channel.BasicAck(ea.DeliveryTag, false); // Ack para descartar a mensagem
                            _retryCountDictionary.TryRemove(content, out _); // Remove do dicionário de retry
                            _sender.SendMessage("pagamento_erro", content);
                        }
                    }
                    else
                    {
                        // Adiciona ao dicionário de retry
                        _retryCountDictionary.TryAdd(content, 1);
                        // Rejeita e reenfileira a mensagem
                        _channel.BasicNack(ea.DeliveryTag, false, true);
                    }
                }
            };

            _channel.BasicConsume("novo_pedido", false, consumer);
        }

        public void ReenqueueMessage(string message)
        {
            EnsureNotDisposed();
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish("novo_pedido_exchange", "novo_pedido.*", null, body);
        }

        private void ConnectRabbitMQ()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password,
                Port = 5671, // Porta para SSL
                Ssl = new SslOption
                {
                    Enabled = true,
                    ServerName = _hostname, // Ou o nome do servidor conforme certificado
                    Version = System.Security.Authentication.SslProtocols.Tls12 // Certifique-se de que a versão TLS é suportada pelo seu servidor
                },
                RequestedConnectionTimeout = TimeSpan.FromSeconds(60), // Timeout de conexão
                SocketReadTimeout = TimeSpan.FromSeconds(60), // Timeout de leitura
                SocketWriteTimeout = TimeSpan.FromSeconds(60), // Timeout de escrita
                DispatchConsumersAsync = true
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare("novo_pedido_exchange", ExchangeType.Direct);
                _channel.QueueDeclare("novo_pedido", false, false, false, null);
                _channel.QueueBind("novo_pedido", "novo_pedido_exchange", "novo_pedido.*", null);
                _channel.BasicQos(0, 1, false);

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                _logger.LogInformation("Connected to RabbitMQ");
            }
            catch (BrokerUnreachableException ex)
            {
                _logger.LogError(ex, "Could not reach the broker");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to connect to RabbitMQ");
                throw;
            }
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            _logger.LogInformation("RabbitMQ connection shutdown.");
        }

        private void EnsureNotDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(PagamentoMessageQueue));
            }
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;

            _channel?.Close();
            _connection?.Close();
        }
    }
}
