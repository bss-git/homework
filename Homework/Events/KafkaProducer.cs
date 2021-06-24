using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Events
{
    public class KafkaProducer
    {
        private ProducerConfig _config;

        public KafkaProducer(IOptions<KafkaOptions> options)
        {
            _config = new ProducerConfig { BootstrapServers = options.Value.BootstrapServers };
        }

        public Task ProduceAsync(string topic, object data)
        {
            using var p = new ProducerBuilder<Null, string>(_config).Build();
            return p.ProduceAsync(topic, new Message<Null, string> { Value = JsonConvert.SerializeObject(data) });
        }
    }
}
