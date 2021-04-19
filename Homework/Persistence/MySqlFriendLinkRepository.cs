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

        public async Task<IEnumerable<Guid>> GetFriendIdsAsync(Guid userId)
        {
            return await _mySql.GetListAsync("GET_FriendLinkFriendIdList", FromReaderList,
                new MySqlParameter("@userId", userId.ToByteArray()));
        }

        private static Guid FromReaderList(MySqlDataReader reader)
        {
            return reader.GetGuid("friendId");
        }

        public Task SaveAsync(Guid friend1, Guid friend2)
        {
            return _mySql.ExecuteNonQueryAsync("INSERT_FriendLink", new[] {
                new MySqlParameter("@friend1", friend1.ToByteArray()),
                new MySqlParameter("@friend2", friend2.ToByteArray())
            });
        }
    }
}
