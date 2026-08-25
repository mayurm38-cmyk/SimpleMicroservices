namespace EmployeeService.Kafka
{
    public interface IKafkaProducer
    {
        Task PublishAsync(string topic, string message);
    }
}