namespace ProducerService.Interfaces
{
    public interface IKafkaPublisher
    {
        Task PublishAsync(string key, string value);
    }
}
