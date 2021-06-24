using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Homework.Events
{
    public class KafkaConsumer
    {
        private string _bootstrapServers;

        public KafkaConsumer(IOptions<KafkaOptions> options)
        {
            _bootstrapServers = options.Value.BootstrapServers;
        }

        public void ConsumeAsync<T>(string topic, Action<T> action, CancellationToken ct)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId = Guid.NewGuid().ToString(),
                AutoOffsetReset = AutoOffsetReset.Latest
            };

            using var c = new ConsumerBuilder<Ignore, string>(config).Build();
            c.Subscribe(topic);

            try
            {
                while (true)
                {
                    try
                    {
                        var data = c.Consume(ct);
                        action(JsonConvert.DeserializeObject<T>(data.Message.Value));
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Error occured: {e.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                c.Close();
            }
        }
    }
}
