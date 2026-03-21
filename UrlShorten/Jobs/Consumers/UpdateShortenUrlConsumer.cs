using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using UrlShorten.Infrastructures.Abstracts;
using UrlShorten.Models.Entities;
using UrlShorten.Persistences;

namespace UrlShorten.Jobs.Consumers
{
    public class UpdateShortenUrlConsumer(IServiceProvider sp) : IHostedService
    {
        private const string UpdateShortenPathQueue = "shorten-link.update";

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var rabbitMqBus = sp.GetRequiredService<IRabbitMqBus>();
            var connection = await rabbitMqBus.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();
            
            await channel.QueueDeclareAsync(UpdateShortenPathQueue, durable: true, exclusive: false, autoDelete: false);
            //await channel.BasicQosAsync(0, 1, false);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                try
                {
                    using var scope = sp.CreateScope();
                    var msg = Encoding.UTF8.GetString(ea.Body.ToArray());
                    ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var afterShortenItem = System.Text.Json.JsonSerializer.Deserialize<ShortenLink>(msg);

                    var shortenLink = await context.ShortenLinks
                        .Where(x => x.Id == afterShortenItem.Id)
                        .FirstOrDefaultAsync(cancellationToken);

                    if(shortenLink is null)
                        throw new NullReferenceException($"ShortenLink with Id {afterShortenItem.Id} not found.");

                    shortenLink.ShortenPath = afterShortenItem.ShortenPath;

                    context.ShortenLinks.Update(shortenLink);
                    await context.SaveChangesAsync();

                    await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    // BasicNack tells RabbitMQ to requeue or send to Dead Letter
                    await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
                }

            };

            await channel.BasicConsumeAsync(queue: UpdateShortenPathQueue,
                autoAck: false,
                consumer: consumer,
                cancellationToken: cancellationToken);

            //await Task.Delay(Timeout.Infinite, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
