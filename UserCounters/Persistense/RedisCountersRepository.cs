using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserCounters.Domain;
using UserCounters.Persistense.Redis;

namespace UserCounters.Persistense
{
    public class RedisCountersRepository : ICountersRepository
    {
        private readonly RedisDb _redisDb;

        public RedisCountersRepository(RedisDb redisDb)
        {
            _redisDb = redisDb;
        }

        public async Task<UserCounter> Get(Guid userId)
        {
            var values = await _redisDb.GetDatabase().HashGetAllAsync(userId.ToString());

            var result = new UserCounter { UserId = userId };

            foreach (var value in values)
            {
                if (value.Name == "dialogs")
                    result.Dialogs = (int) value.Value;

                if (value.Name == "updates")
                    result.Updates = (int)value.Value;
            }

            return result;
        }
    }
}
