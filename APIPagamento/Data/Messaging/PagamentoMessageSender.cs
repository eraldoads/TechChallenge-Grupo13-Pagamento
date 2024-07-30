﻿using Domain.Interfaces;
using RabbitMQ.Client;
using System;
using System.Text;

namespace Data.Messaging
{
    public class PagamentoMessageSender : IPagamentoMessageSender
    {
        private readonly string _hostname = Environment.GetEnvironmentVariable("RABBIT_HOSTNAME");
        private readonly string _username = Environment.GetEnvironmentVariable("RABBIT_USERNAME");
        private readonly string _password = Environment.GetEnvironmentVariable("RABBIT_PASSWORD");

        public PagamentoMessageSender()
        {
        }

        public void SendMessage(string queueName, string message)
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
                SocketWriteTimeout = TimeSpan.FromSeconds(60) // Timeout de escrita
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: queueName,
                                     basicProperties: null,
                                     body: body);

                Console.WriteLine($" [x] Sent {message}");
            }
        }
    }
}