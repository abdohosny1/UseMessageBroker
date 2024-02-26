namespace ConsumerService.Interfaces
{
    public interface IKafkaConsumer
    {
        void Consume(Action<string, string> messageHandler, CancellationToken cancellationToken);
    }
}
