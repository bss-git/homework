using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Dialogs.Persistence.Mysql
{
    public class MySqlDb
    {
        private ILogger<MySqlDb> _logger;

        public MySqlDb(ILogger<MySqlDb> logger)
        {
            _logger = logger;
        }

        public async Task ExecuteNonQueryAsync(string connectionString, string procedureName, params MySqlParameter[] parameters)
        {
            try
            {
                using var conn = new MySqlConnection(connectionString);
                var cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = procedureName;

                await conn.OpenAsync();

                foreach (var param in parameters)
                {
                    cmd.Parameters.Add(param);
                }

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<List<T>> GetListAsync<T>(string connectionString, string procedureName, Func<MySqlDataReader, T> fromReader, params MySqlParameter[] parameters)
        {
            try
            {
                await using var conn = new MySqlConnection(connectionString);
                var cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = procedureName;

                foreach (var param in parameters)
                {
                    cmd.Parameters.Add(param);
                }

                await conn.OpenAsync();

                var reader = (MySqlDataReader)await cmd.ExecuteReaderAsync();
                var result = new List<T>();

                while (reader.Read())
                {
                    result.Add(fromReader(reader));
                }

                return result;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
