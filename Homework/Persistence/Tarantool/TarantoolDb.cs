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
            _connectionString = options.Value.ConnectionsString;
        }
        public async Task<TValue> GetItemAsync<TKey, TValue>(string spaceName, TKey key)
        {
            using var box = await Box.Connect(_connectionString);
            var schema = box.GetSchema();
            var space = schema[spaceName];
            var response = await space.Select<TKey, TValue>(key);

            return response.Data.FirstOrDefault();
        }

        public async Task InsertAsync<T>(string spaceName, T value)
        {
            using var box = await Box.Connect(_connectionString);
            var schema = box.GetSchema();
            var space = schema[spaceName];
            await space.Insert(value);
        }

        public async Task DeleteAsync<TKey>(string spaceName, TKey key)
        {
            using var box = await Box.Connect(_connectionString);
            var schema = box.GetSchema();
            var space = schema[spaceName];
            await space.Delete<ValueTuple<TKey>, object>(ValueTuple.Create(key));
        }
    }
}
