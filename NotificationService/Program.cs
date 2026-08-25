using Microsoft.Extensions.Hosting;
using NotificationService.Kafka;
using NotificationService.Services;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;

namespace NotificationService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddSingleton<EmailService>();

        builder.Services.AddHostedService<NotificationConsumer>();

        var host = builder.Build();

        host.Run();
    }
}
