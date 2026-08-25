using Confluent.Kafka;
using System.Text.Json;
using EmployeeService.Event;

namespace EmployeeService.Kafka
{
    public class KafkaConsumer : BackgroundService
    {
        private readonly ILogger<KafkaConsumer> _logger;


        public KafkaConsumer(ILogger<KafkaConsumer> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(
     CancellationToken stoppingToken)
        {
            Console.WriteLine("🔥🔥 CONSUMER STARTED 🔥🔥");

            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "employee-service-dotnet-test",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };

            Console.WriteLine("🔥 Creating Kafka Consumer...");

            using var consumer =
                new ConsumerBuilder<Ignore, string>(config)
                    .Build();

            Console.WriteLine("🔥 Kafka Consumer Created");

            consumer.Subscribe("employee-events");

            Console.WriteLine("🔥 Subscribed to employee-events");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    Console.WriteLine("🔥 Waiting for Kafka message...");

                    var result = consumer.Consume(stoppingToken);

                    if (result?.Message?.Value == null)
                        continue;

                    Console.WriteLine(
                        $"🔥🔥 MESSAGE RECEIVED: {result.Message.Value}");

                    var employeeEvent =
                        JsonSerializer.Deserialize<EmployeeCreatedEvent>(
                            result.Message.Value);

                    if (employeeEvent != null)
                    {
                        Console.WriteLine(
                            $"🔥 Employee Created - Id: {employeeEvent.Id}, " +
                            $"Name: {employeeEvent.Name}, " +
                            $"Salary: {employeeEvent.Salary}, " +
                            $"Address: {employeeEvent.Address}, " +
                            $"Email: {employeeEvent.Email}"
                            );
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Consumer stopping...");
            }
            finally
            {
                consumer.Close();
            }

            await Task.CompletedTask;
        }
    }
}