using ConsumerService.Data;
using ConsumerService.Model;
using System.Text.Json;
using UseRabbitMQ;

namespace ConsumerService.BackgroundServices
{
    public class RabbitMQBackgroundService : BackgroundService
    {
        private readonly RabbitMQConsumer _consumer;
        private readonly IServiceScopeFactory _scopeFactory;

        public RabbitMQBackgroundService(RabbitMQConsumer consumer, IServiceScopeFactory scopeFactory)
        {
            _consumer = consumer;
            _scopeFactory = scopeFactory;
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Consume(async message =>
            {
                await ProcessMessage(message);
            });

            // Return a Task that completes when the stoppingToken is triggered
            return Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, stoppingToken);
                }
            }, stoppingToken);
        }

        private async Task ProcessMessage(string message)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                Console.WriteLine($"Received message: {message}");
                var model = JsonSerializer.Deserialize<SendMessageInQueue>(message);
                dbContext.ReceiveMessages.Add(new ReceiveMessage { Name = model.Name });
                await dbContext.SaveChangesAsync();
            }
        }
    }

}
