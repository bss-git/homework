using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserCounters.Domain;
using UserCounters.Persistense.Redis;

namespace UserCounters.IncomingEvents.Handlers
{
    public class DialogsEventHandler : IncomingEventHandler
    {
        public DialogsEventHandler(RedisDb redisDb) : base(redisDb) { }

        protected override async Task HandleInner(ConsumeResult<Guid, string> consumeResult, ITransaction redisTran)
        {
            var counterEvent = JsonConvert.DeserializeObject<UserCounterEvent>(consumeResult.Message.Value);
            if (counterEvent.EventType == EventType.NewMessage)
                await redisTran.HashIncrementAsync(counterEvent.UserId.ToString(), "dialogs");
            else
                await redisTran.HashSetAsync(counterEvent.UserId.ToString(), "dialogs", 0);
        }
    }
}
