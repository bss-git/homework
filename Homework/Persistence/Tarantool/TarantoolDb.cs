using Microsoft.Extensions.Options;
using ProGaudi.Tarantool.Client;
using ProGaudi.Tarantool.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Persistence.Tarantool
{
    public class TarantoolDb
    {
        private TarantoolConnectionPool _pool;

        public TarantoolDb(IOptions<TaratoolOptions> options)
        {
            _pool = new TarantoolConnectionPool(options.Value.ConnectionString);
        }
        public async Task<TResult[]> CallAsync<TParam, TResult>(string procedureName, TParam parameters)
        {
            return (await _pool.Box.Call<TParam, TResult>(procedureName, parameters)).Data;
        }

        public async Task CallAsync<TParam>(string procedureName, TParam parameters)
        {
            await _pool.Box.Call(procedureName, parameters);
        }
    }
}
