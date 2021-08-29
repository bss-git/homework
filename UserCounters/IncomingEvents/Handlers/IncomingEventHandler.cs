using Confluent.Kafka;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserCounters.Persistense.Redis;

namespace UserCounters.IncomingEvents.Handlers
{
    public abstract class IncomingEventHandler
    {
        private readonly RedisDb _redisDb;

        public IncomingEventHandler(RedisDb redisDb)
        {
            _redisDb = redisDb;
        }

        public async Task Handle(ConsumeResult<string, string> consumeResult)
        {
            var db = _redisDb.GetDatabase();

            var isHandled = await db.SetContainsAsync("updates:handled", consumeResult.Message.Key);
            if (isHandled)
                return;
            
            var tran = db.CreateTransaction();

            await HandleInner(consumeResult, tran);

            tran.SetAddAsync("updates:handled", consumeResult.Message.Key.ToString());

            await tran.ExecuteAsync();
        }

        protected abstract Task HandleInner(ConsumeResult<string, string> consumeResult, ITransaction redisTran);
    }
}
