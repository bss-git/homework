using Homework.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Homework.Persistence
{
    public class HashingMySqlPasswordManager : IPasswordManager
    {
        private MySqlDb _mySql;

        public HashingMySqlPasswordManager(MySqlDb mySqlDb)
        {
            _mySql = mySqlDb;
        }

        public async Task<bool> IsValidPasswordAsync(string login, string password)
        {
            var passwordSalt = await _mySql.GetItemAsync<UserPassword>("GET_UserPassword",
                reader => new UserPassword
                {
                    PasswordHash = (byte[]) reader["PasswordHash"],
                    Salt = reader.GetString("Salt")
                }, new MySqlParameter("@login", login));

            if (passwordSalt == null)
                return false;

            var hashed = CreateHash(password, passwordSalt.Salt);

            return hashed.SequenceEqual(passwordSalt.PasswordHash);
        }

        private static byte[] CreateHash(string input, string salt)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(input + salt));
        }

        public async Task SavePasswordAsync(string login, string password)
        {
            var salt = Guid.NewGuid().ToString();
            var hash = CreateHash(password, salt);

            await _mySql.ExecuteNonQueryAsync("INUPD_UserPassword", new[] {
                new MySqlParameter("@login", login),
                new MySqlParameter("@passwordHash", hash),
                new MySqlParameter("@salt", salt)
            });
        }
    }
}
