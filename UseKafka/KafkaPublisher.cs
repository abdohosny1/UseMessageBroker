using Confluent.Kafka;

namespace UseKafka
{
    public class KafkaPublisher
    {
        private readonly string _bootstrapServers;
        private readonly string _topic;

        public KafkaPublisher(string bootstrapServers, string topic)
        {
            _bootstrapServers = bootstrapServers;
            _topic = topic;
        }

        public async Task PublishAsync(string key, string value)
        {
            var config = new ProducerConfig { BootstrapServers = _bootstrapServers };

            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                await producer.ProduceAsync(_topic, new Message<string, string> { Key = key, Value = value });
            }
        }
    }
}