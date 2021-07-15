using Homework.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data.Common;
using Homework.Friends;
using Homework.Persistence;
using Homework.Dialogs.Application;
using Homework.Dialogs.Application.Dto;

namespace Homework.Dialogs.Persistense
{
    public class MySqlDialogsRepository : IDialogsRepository
    {
        private MySqlDb _mySql;
        private readonly DialogsShardSelector _shardSelector;

        public MySqlDialogsRepository(MySqlDb mySqlDb, DialogsShardSelector shardSelector)
        {
            _mySql = mySqlDb;
            _shardSelector = shardSelector;
        }

        private static Message FromReader(MySqlDataReader reader)
        {
            return new Message(reader.GetGuid("Id"),
                reader.GetGuid("From"),
                reader.GetGuid("To"),
                reader.GetString("Text"),
                reader.GetDateTime("Timestamp"));
        }

        public Task SaveAsync(Message message)
        {
            return _mySql.ExecuteNonQueryAsync(_shardSelector.GetConnectionString(message.From, message.To),
                "INSERT_Messages", new[] {
                new MySqlParameter("@id", message.Id.ToByteArray()),
                new MySqlParameter("@from", message.From.ToByteArray()),
                new MySqlParameter("@to", message.To.ToByteArray()),
                new MySqlParameter("@text", message.Text),
                new MySqlParameter("@timestamp", message.Timestamp)
            });
        }

        public async Task<IEnumerable<Message>> GetListAsync(Guid user1, Guid user2)
        {
            return await _mySql.GetListAsync(_shardSelector.GetConnectionString(user1, user2),
                "GET_MessagesList", FromReader, new[] {
                new MySqlParameter("@user1", user1.ToByteArray()),
                new MySqlParameter("@user2", user2.ToByteArray()) });
        }
    }
}
