using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using UrlShorten.Infrastructures.Abstracts;
using UrlShorten.Models.Enums.RabbitMQ;

namespace UrlShorten.Infrastructures.Implements
{
    public class RabbitMqBus : IRabbitMqBus
    {
        private readonly IConfiguration _configuration;

        public RabbitMqBus(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IConnection> CreateConnectionAsync()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["MessageBus:Host"],
                UserName = _configuration["MessageBus:UserName"],
                Password = _configuration["MessageBus:Password"],
                Port = int.Parse(_configuration["MessageBus:Port"])
            };

            return await factory.CreateConnectionAsync();
        }

        public async Task PublishAsync<T>(T message, string queueName) where T : class
        {
            using var connection = await CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queueName,
                durable: true,
                exclusive: false,
                autoDelete: false);

            // use default exchange to publish the message to the specified queue
            await channel.BasicPublishAsync(
                string.Empty,   // Default exchange
                queueName,
                Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message)));
        }

        public async Task PublishAsync<T>(T message, string queueName, string exchangeName, AppExchangeModes exchangeMode, string routingKey) where T : class
        {
            using var connection = await CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queueName,
                durable: true,
                exclusive: false,
                autoDelete: false);

            await channel.ExchangeDeclareAsync(exchangeName, AppExchangeModes.Fanout.ToString().ToLower(), durable: true, autoDelete: false);

            await channel.QueueBindAsync(queueName,
                exchangeName,
                routingKey: routingKey);

            // use default exchange to publish the message to the specified queue
            await channel.BasicPublishAsync(
                exchangeName,
                routingKey,
                Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message)));
        }
    }
}
