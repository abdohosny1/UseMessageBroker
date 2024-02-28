using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseRabbitMQ
{
    public class RabbitMQPublisher
    {
        private readonly string _hostname;
        private readonly string _queueName;

        public RabbitMQPublisher(string hostname, string queueName)
        {
            _hostname = hostname;
            _queueName = queueName;
        }

        public void Publish(IntegrationEvent message)
        {
            var factory = new ConnectionFactory() { HostName = _hostname };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: _queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                // Serialize the IntegrationEvent object to JSON
                string messageJson = JsonConvert.SerializeObject(message); // For Newtonsoft.Json
                                                                           // Or use the following line for System.Text.Json
                                                                           // string messageJson = JsonSerializer.Serialize(message);

                var body = Encoding.UTF8.GetBytes(messageJson);

                channel.BasicPublish(exchange: "",
                                     routingKey: _queueName,
                                     basicProperties: null,
                                     body: body);
            }
        }

    }
}
