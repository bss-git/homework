using Homework.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data.Common;
using Homework.Users.Dto;

namespace Homework.Persistence
{
    public class MySqlUserRepository : IUserRepository
    {
        private MySqlDb _mySql;

        public MySqlUserRepository(MySqlDb mySqlDb)
        {
            _mySql = mySqlDb;
        }

        public Task<User> GetAsync(string login)
        {
            return _mySql.GetItemAsync("GET_UserByLogin", FromReader, new MySqlParameter("@login", login));
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

        private static UserListViewModel FromReaderList(MySqlDataReader reader)
        {
            return new UserListViewModel
            {
                Id = reader.GetGuid("Id"),
                Login = reader.GetString("Login"),
                City = reader.GetString("City"),
                Name = reader.GetString("Name"),
                Surname = reader.GetString("Surname")
            };
        }

        public Task SaveAsync(User user)
        {
            return _mySql.ExecuteNonQueryAsync("INUPD_User", new[] {
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

        public async Task<IEnumerable<UserListViewModel>> GetListAsync(int offset, int limit)
        {
            return await _mySql.GetListAsync("GET_UserList", FromReaderList,
                new MySqlParameter("@offset", offset),
                new MySqlParameter("@limit", limit));
        }

        public Task<User> GetAsync(Guid id)
        {
            return _mySql.GetItemAsync("GET_UserById", FromReader, new MySqlParameter("@id", id.ToByteArray()));
        }

        public async Task<IEnumerable<UserListViewModel>> GetFriendsAsync(Guid userId)
        {
            return await _mySql.GetListAsync("GET_UserFriends", FromReaderList,
                new MySqlParameter("@userId", userId.ToByteArray()));
        }

        public async Task<IEnumerable<UserListViewModel>> GetOfferedFriendsAsync(Guid userId)
        {
            return await _mySql.GetListAsync("GET_UserOfferedFriends", FromReaderList,
                new MySqlParameter("@userId", userId.ToByteArray()));
        }
    }
}
