using Microsoft.Extensions.Logging;
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
        private string _masterConnectionString;
        private string _replicasConnectionString;
        private ILogger<MySqlDb> _logger;

        public MySqlDb(IOptions<MySqlOptions> options, ILogger<MySqlDb> logger)
        {
            _logger = logger;

            var sb = new MySqlConnectionStringBuilder();
            sb.Server = options.Value.MasterHost;
            sb.Pooling = true;
            sb.UserID = options.Value.User;
            sb.Password = options.Value.Password;
            sb.Database = options.Value.Database;

            _masterConnectionString = sb.ConnectionString;

            sb = new MySqlConnectionStringBuilder();
            sb.Server = options.Value.ReadReplicas;
            sb.Pooling = true;
            sb.UserID = options.Value.User;
            sb.Password = options.Value.Password;
            sb.Database = options.Value.Database;

            _replicasConnectionString = sb.ConnectionString;
        }

        public Task ExecuteNonQueryAsync(string procedureName, params MySqlParameter[] parameters)
        {
            return ExecuteNonQueryAsync(_masterConnectionString, procedureName, parameters);
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

        public async Task ExecuteText(string text)
        {
            try
            {
                using var conn = new MySqlConnection(_masterConnectionString);
                var cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = text;

                await conn.OpenAsync();

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        //public Task<List<T>> GetListFromReplicaAsync<T>(string procedureName, Func<MySqlDataReader, T> fromReader, params MySqlParameter[] parameters)
        //{
        //    return GetListAsync<T>(procedureName, fromReader, parameters, true);
        //}

        //public Task<List<T>> GetListAsync<T>(string procedureName, Func<MySqlDataReader, T> fromReader, params MySqlParameter[] parameters)
        //{
        //    return GetListAsync<T>(procedureName, fromReader, parameters, false);
        //}

        public Task<List<T>> GetListAsync<T>(string procedureName, Func<MySqlDataReader, T> fromReader, params MySqlParameter[] parameters)
        {
            return GetListAsync<T>(_replicasConnectionString, procedureName, fromReader, parameters);
        }

        public async Task<List<T>> GetListAsync<T>(string connectionString, string procedureName, Func<MySqlDataReader, T> fromReader, params MySqlParameter[] parameters)
        {
            try
            {
                //await using var conn = useReplicas ? new MySqlConnection(_replicasConnectionString) : new MySqlConnection(_clusterConnectionString);
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

        public async Task<T> GetItemAsync<T>(string procedureName, Func<MySqlDataReader, T> fromReader, params MySqlParameter[] parameters)
        {
            try
            {
                await using var conn = new MySqlConnection(_replicasConnectionString);
                var cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = procedureName;

                foreach (var param in parameters)
                {
                    cmd.Parameters.Add(param);
                }

                await conn.OpenAsync();

                var reader = (MySqlDataReader)await cmd.ExecuteReaderAsync();

                if (reader.Read())
                    return fromReader(reader);

                return default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
