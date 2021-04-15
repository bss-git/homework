using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Persistence
{
    public static class MySqlDb
    {
        private static string _connectionString;

        public static void SetConnectionParams(string host, string database, string user, string password)
        {
            _connectionString = "Database=" + database + ";Datasource=" + host + ";User=" + user + ";Password=" + password;
        }

        public static async Task ExecuteNonQueryAsync(string procedureName, params MySqlParameter[] parameters)
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

        public static async Task<List<T>> GetListAsync<T>(string procedureName, Func<MySqlDataReader, T> fromReader, params MySqlParameter[] parameters)
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

        public static async Task<T> GetItemAsync<T>(string procedureName, Func<MySqlDataReader, T> fromReader, params MySqlParameter[] parameters)
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
