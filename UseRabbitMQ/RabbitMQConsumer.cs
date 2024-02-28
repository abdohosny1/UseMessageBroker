using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace UseRabbitMQ
{
    public class RabbitMQConsumer
    {
        private readonly string _hostname;
        private readonly string _queueName;

        public RabbitMQConsumer(string hostname, string queueName)
        {
            _hostname = hostname;
            _queueName = queueName;
        }

        public void Consume(Action<string> handleMessage)
        {
            var factory = new ConnectionFactory() { HostName = _hostname };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                handleMessage(message);
            };

            channel.BasicConsume(queue: _queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
