using Homework.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data.Common;
using Homework.Friends;
using Homework.Updates;
using Homework.Updates.Dto;

namespace Homework.Persistence
{
    public class MySqlUpdatesRepository : IUpdatesRepository
    {
        private MySqlDb _mySql;

        public MySqlUpdatesRepository(MySqlDb mySqlDb)
        {
            _mySql = mySqlDb;
        }

        public async Task<IEnumerable<UpdateViewModel>> GetListAsync(Guid userId)
        {
            return await _mySql.GetListAsync("GET_UpdatesList", FromReaderList,
                new MySqlParameter("@userId", userId.ToByteArray()));
        }

        public Task SaveAsync(UpdateViewModel update)
        {
            return _mySql.ExecuteNonQueryAsync("INSERT_Update",
                new MySqlParameter("@userId", update.UserId.ToByteArray()),
                new MySqlParameter("@userName", update.UserName),
                new MySqlParameter("@timestamp", update.Timestamp),
                new MySqlParameter("@message", update.Message)
            );
        }

        private static UpdateViewModel FromReaderList(MySqlDataReader reader)
        {
            return new UpdateViewModel
            {
                UserId = reader.GetGuid("UserId"),
                UserName = reader.GetString("UserName"),
                Timestamp = reader.GetDateTime("Timestamp"),
                Message = reader.GetString("Message")
            };
        }
    }
}
