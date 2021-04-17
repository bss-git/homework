using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Persistence
{
    public class MySqlDb
    {
        private string _connectionString;

        public MySqlDb(IOptions<MySqlOptions> options)
        {
            _connectionString = "Database=" + options.Value.Database
                + ";Datasource=" + options.Value.Host
                + ";User=" + options.Value.User
                + ";Password=" + options.Value.Password;
        }

        public async Task ExecuteNonQueryAsync(string procedureName, params MySqlParameter[] parameters)
        {
            using var conn = new MySqlConnection(_connectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = procedureName;

            await conn.OpenAsync();

            foreach(var param in parameters)
            {
                cmd.Parameters.Add(param);
            }

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<T>> GetListAsync<T>(string procedureName, Func<MySqlDataReader, T> fromReader, params MySqlParameter[] parameters)
        {
            await using var conn = new MySqlConnection(_connectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = procedureName;

            foreach (var param in parameters)
            {
                cmd.Parameters.Add(param);
            }

            await conn.OpenAsync();

            var reader = (MySqlDataReader) await cmd.ExecuteReaderAsync();
            var result = new List<T>();

            while (reader.Read())
            {
                result.Add(fromReader(reader));
            }

            return result;
        }

        public async Task<T> GetItemAsync<T>(string procedureName, Func<MySqlDataReader, T> fromReader, params MySqlParameter[] parameters)
        {
            await using var conn = new MySqlConnection(_connectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = procedureName;

            foreach (var param in parameters)
            {
                cmd.Parameters.Add(param);
            }

            await conn.OpenAsync();

            var reader = (MySqlDataReader) await cmd.ExecuteReaderAsync();

            if (reader.Read())
                return fromReader(reader);

            return default;
        }
    }
}
