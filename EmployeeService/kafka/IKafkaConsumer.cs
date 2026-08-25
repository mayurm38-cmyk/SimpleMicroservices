namespace EmployeeService.Kafka
{
    public interface IKafkaConsumer
    {
        Task StartAsync(CancellationToken cancellationToken);
    }
}