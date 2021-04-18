using Homework.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data.Common;
using Homework.Friends;

namespace Homework.Persistence
{
    public class MySqlFriendLinkRepository : IFriendLinkRepository
    {
        private MySqlDb _mySql;

        public MySqlFriendLinkRepository(MySqlDb mySqlDb)
        {
            _mySql = mySqlDb;
        }

        public Task SaveAsync(Guid friend1, Guid friend2)
        {
            return _mySql.ExecuteNonQueryAsync("INSERT_FriendLink", new[] {
                new MySqlParameter("@friend1", friend1.ToByteArray()),
                new MySqlParameter("@friend2", friend1.ToByteArray())
            });
        }
    }
}
