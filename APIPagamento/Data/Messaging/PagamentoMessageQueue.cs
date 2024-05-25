using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Data.Messaging
{
    public class PagamentoMessageQueue : IPagamentoMessageQueue
    {
        private readonly ILogger _logger;
        private IConnection _connection;
        private IModel _channel;

        public PagamentoMessageQueue(ILogger<PagamentoMessageQueue> logger) 
        {
            _logger = logger;
            ConnectRabbitMQ();
        }

        public Task ReceberMensagem()
        {            
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                // received message
                string content = Encoding.UTF8.GetString(ea.Body.ToArray());

                // handle the received message
                HandleMessage(content);
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            _channel.BasicConsume("pagamento_pendente", false, consumer);
            return Task.CompletedTask;
        }

        private void ConnectRabbitMQ()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            
            _connection = factory.CreateConnection();            
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare("pagamento_exchange", ExchangeType.Direct);
            _channel.QueueDeclare("pagamento_pendente", false, false, false, null);
            _channel.QueueBind("pagamento_pendente", "pagamento_exchange", "pagamento_pendente.*", null);
            _channel.BasicQos(0, 1, false);

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }



        private void HandleMessage(string content)
        {            
            _logger.LogInformation($"Mensagem recebida /n{content}");
        }

        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e) { }
        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerRegistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerShutdown(object sender, ShutdownEventArgs e) { }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e) { }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();            
        }
    }
}
