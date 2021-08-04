using Confluent.Kafka;
using System;
using UserCounters.IncomingEvents.Handlers;
using UserCounters.Persistense.Redis;

namespace UserCounters.IncomingEvents
{
    public class EventHandlerFactory
    {
        private readonly RedisDb _redisDb;

        public EventHandlerFactory(RedisDb redisDb)
        {
            _redisDb = redisDb;
        }

        public IncomingEventHandler GetHandler(ConsumeResult<Guid, string> cosumeResult)
        {
            if (cosumeResult.Topic == "user_dialog")
                return new DialogsEventHandler(_redisDb);

            if (cosumeResult.Topic == "user_update")
                return new UpdatesEventHandler(_redisDb);

            throw new InvalidOperationException();
        }
    }
}