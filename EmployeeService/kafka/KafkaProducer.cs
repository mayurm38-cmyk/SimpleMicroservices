using Confluent.Kafka;

namespace EmployeeService.Kafka
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly IConfiguration _configuration;

        public KafkaProducer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task PublishAsync(string topic, string message)
        {
            var bootstrapServers =
                _configuration["Kafka:BootstrapServers"];

            var config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers
            };

            using var producer =
                new ProducerBuilder<Null, string>(config).Build();

            await producer.ProduceAsync(
                topic,
                new Message<Null, string>
                {
                    Value = message
                });
        }
    }
}