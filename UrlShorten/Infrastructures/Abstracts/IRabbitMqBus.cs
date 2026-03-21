using RabbitMQ.Client;
using UrlShorten.Models.Enums.RabbitMQ;

namespace UrlShorten.Infrastructures.Abstracts
{
    public interface IRabbitMqBus : IMessageBus
    {
        Task<IConnection> CreateConnectionAsync();

        Task PublishAsync<T>(T message, string queueName, string exchangeName, AppExchangeModes exchangeMode, string routingKey) where T : class;
    }
}
