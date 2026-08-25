using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotificationService.Event;
using NotificationService.Services;
using System.Text.Json;

namespace NotificationService.Kafka
{
    public class NotificationConsumer : BackgroundService
    {
        private readonly ILogger<NotificationConsumer> _logger;
        private readonly EmailService _emailService;

        public NotificationConsumer(
            ILogger<NotificationConsumer> logger,
            EmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "notification-service_v2",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };

            using var consumer =
                new ConsumerBuilder<Ignore, string>(config)
                    .Build();

            consumer.Subscribe("employee-events");

            Console.WriteLine(
                "🔥 Notification Consumer Started...");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    Console.WriteLine(
                        "Waiting for EmployeeCreated event...");

                    var result =
                        consumer.Consume(stoppingToken);

                    if (result?.Message?.Value == null)
                        continue;

                    Console.WriteLine(
                        $"🔥 Event Received: {result.Message.Value}");

                    var employee =
                        JsonSerializer.Deserialize<EmployeeCreatedEvent>(
                            result.Message.Value);

                    if (employee != null)
                    {
                        _logger.LogInformation(
                            "Employee Created - Id: {Id}, Name: {Name}, Email: {Email}",
                            employee.Id,
                            employee.Name,
                            employee.Email);

                        // Old Kafka messages may not contain Email
                        if (string.IsNullOrWhiteSpace(employee.Email))
                        {
                            _logger.LogWarning(
                                "⚠️ Email is missing for Employee Id: {Id}. Skipping email notification.",
                                employee.Id);

                            continue;
                        }

                        _logger.LogInformation(
                            "📧 Sending email to {Email}",
                            employee.Email);

                        await _emailService.SendEmployeeCreatedEmailAsync(
                            employee.Name,
                            employee.Email);

                        _logger.LogInformation(
                            "✅ Email sent successfully to {Email}",
                            employee.Email);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine(
                    "Notification Consumer stopping...");
            }
            finally
            {
                consumer.Close();
            }
        }
    }
}