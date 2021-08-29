using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserCounters.Persistense.Redis
{
    public class RedisDb
    {
        private readonly ConnectionMultiplexer _multiplexer;

        public RedisDb(IOptions<RedisOptions> options)
        {
            _multiplexer = ConnectionMultiplexer.Connect($"{options.Value.Host}:{options.Value.Port}");
        }

        public IDatabase GetDatabase()
        {
            return _multiplexer.GetDatabase(0);
        }

        
    }
}
