using Microsoft.Extensions.Options;
using ProGaudi.Tarantool.Client;
using ProGaudi.Tarantool.Client.Model.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Persistence.Tarantool
{
    public class TarantoolDb
    {
        private string _connectionString;

        public TarantoolDb(IOptions<TaratoolOptions> options)
        {
            _connectionString = options.Value.ConnectionString;
        }
        public async Task<TResult[]> CallAsync<TParam, TResult>(string procedureName, TParam parameters)
        {
            using var box = await Box.Connect(_connectionString);
            return (await box.Call<TParam, TResult>(procedureName, parameters)).Data;
        }

        public async Task CallAsync<TParam>(string procedureName, TParam parameters)
        {
            using var box = await Box.Connect(_connectionString);
            await box.Call(procedureName, parameters);
        }
    }
}
