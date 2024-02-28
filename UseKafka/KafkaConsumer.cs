using Confluent.Kafka;

namespace UseKafka
{
    public class KafkaConsumer
    {
        private readonly string _bootstrapServers;
        private readonly string _topic;
        private readonly string _groupId;

        public KafkaConsumer(string bootstrapServers, string topic, string groupId)
        {
            _bootstrapServers = bootstrapServers;
            _topic = topic;   
            _groupId = groupId;
        }

        public void Consume(Action<string, string> messageHandler, CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId = _groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var consumer = new ConsumerBuilder<string, string>(config).Build())
            {
                consumer.Subscribe(_topic);

                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var consumeResult = consumer.Consume(cancellationToken);

                        messageHandler(consumeResult.Message.Key, consumeResult.Message.Value);
                    }
                }
                catch (OperationCanceledException)
                {
                    consumer.Close(); // Ensure the consumer leaves the group cleanly and final offsets are committed.
                }
            }
        }
    }
}