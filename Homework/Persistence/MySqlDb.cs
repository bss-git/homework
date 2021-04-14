using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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

        public static string Test()
        {
            using var conn = new MySqlConnection(_connectionString);
            var query = conn.CreateCommand();
            query.CommandText = "SELECT user FROM user;";
            conn.Open();
            var reader = query.ExecuteReader();
            var result = "";

            while (reader.Read())
            {
                result += reader.GetString(0);
            }

            return result;
        }
    }
}
