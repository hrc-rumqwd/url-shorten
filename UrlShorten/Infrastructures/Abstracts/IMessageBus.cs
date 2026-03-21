namespace UrlShorten.Infrastructures.Abstracts
{
    public interface IMessageBus
    {
        Task PublishAsync<T>(T message, string queueName) where T : class;
    }
}
