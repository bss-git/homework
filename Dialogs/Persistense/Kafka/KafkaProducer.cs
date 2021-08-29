using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dialogs.Persistence.Kafka
{
    public class KafkaProducer<TKey>
    {
        private IProducer<TKey, string> _producer;

        public KafkaProducer(IOptions<KafkaOptions> options)
        {
            var config = new ProducerConfig { BootstrapServers = options.Value.BootstrapServers };
            _producer = new ProducerBuilder<TKey, string>(config).Build();
        }

        public Task ProduceAsync(string topic, TKey key, object data)
        {
            return _producer.ProduceAsync(topic, new Message<TKey, string> { Key = key, Value = JsonConvert.SerializeObject(data) });
        }

        public Task ProduceAsync(string topic, TKey key, string data)
        {
            return _producer.ProduceAsync(topic, new Message<TKey, string> { Key = key, Value = data });
        }
    }
}
