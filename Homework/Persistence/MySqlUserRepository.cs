using Homework.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace Homework.Persistence
{
    public class MySqlUserRepository : IUserRepository
    {
        public Task<User> GetAsync(string login)
        {
            return MySqlDb.GetItemAsync("GET_User", FromReader, new MySqlParameter("login", login));
        }

        private static User FromReader(MySqlDataReader reader)
        {
            return new User(reader.GetGuid("Id"), reader.GetString("Login"))
            {
                BirthDate = reader.GetDateTime("BirthDate"),
                City = reader.GetString("City"),
                Gender = (Gender) reader.GetByte("Gender"),
                Interest = reader.GetString("Interest"),
                Name = reader.GetString("Name"),
                Surname = reader.GetString("Surname")
            };
        }

        public Task SaveAsync(User user)
        {
            return MySqlDb.ExecuteNonQueryAsync("INUPD_User", new[] {
                new MySqlParameter("@id", user.Id.ToByteArray()),
                new MySqlParameter("@login", user.Login),
                new MySqlParameter("@city", user.City),
                new MySqlParameter("@gender", (short) user.Gender),
                new MySqlParameter("@interest", user.Interest),
                new MySqlParameter("@birthDate", user.BirthDate),
                new MySqlParameter("@name", user.Name),
                new MySqlParameter("@surname", user.Surname)
            });
        }
    }
}
