using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SharedDto.UserCounters;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserCounters.Domain;
using UserCounters.Persistense.Redis;

namespace UserCounters.IncomingEvents.Handlers
{
    public class UpdatesEventHandler : IncomingEventHandler
    {
        public UpdatesEventHandler(RedisDb redisDb) : base(redisDb) { }

        protected override async Task HandleInner(ConsumeResult<string, string> consumeResult, ITransaction redisTran)
        {
            var counterEvent = JsonConvert.DeserializeObject<UserCounterEvent>(consumeResult.Message.Value);
            if (counterEvent.EventType == EventType.NewMessage)
                redisTran.HashIncrementAsync(counterEvent.UserId.ToString(), "updates");
            else
                redisTran.HashSetAsync(counterEvent.UserId.ToString(), "updates", 0);
        }
    }
}
