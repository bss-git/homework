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
        private IProducer<Null,string> _producer;

        public KafkaProducer(IOptions<KafkaOptions> options)
        {
            var config = new ProducerConfig { BootstrapServers = options.Value.BootstrapServers };
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public Task ProduceAsync(string topic, object data)
        {
            return _producer.ProduceAsync(topic, new Message<Null, string> { Value = JsonConvert.SerializeObject(data) });
        }
    }
}
