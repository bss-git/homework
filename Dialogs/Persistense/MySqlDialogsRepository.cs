using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data.Common;
using Dialogs.Application;
using Dialogs.Application.Dto;
using Microsoft.Extensions.Logging;
using Dialogs.Persistence.Mysql;

namespace Dialogs.Persistence
{
    public class MySqlDialogsRepository : IDialogsRepository
    {
        private MySqlDb _mySql;
        private readonly DialogsShardSelector _shardSelector;
        private readonly ILogger<MySqlDialogsRepository> _logger;

        public MySqlDialogsRepository(MySqlDb mySqlDb, DialogsShardSelector shardSelector, ILogger<MySqlDialogsRepository> logger)
        {
            _mySql = mySqlDb;
            _shardSelector = shardSelector;
            _logger = logger;

            Task.Run(Reshard);
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
                new MySqlParameter("@timestamp", message.Timestamp),
                new MySqlParameter("@hashCode", _shardSelector.GetHashCode(message.From, message.To))
            });
        }

        public async Task<IEnumerable<Message>> GetListAsync(Guid user1, Guid user2)
        {
            return (await _mySql.GetListAsync(_shardSelector.GetConnectionString(user1, user2),
                "GET_MessagesList", FromReader, new[] {
                new MySqlParameter("@user1", user1.ToByteArray()),
                new MySqlParameter("@user2", user2.ToByteArray()) }))
                .OrderBy(x => x.Timestamp);
        }

        private async Task Reshard()
        {
            _logger.LogInformation("start resharding");
            var shards = _shardSelector.GetShards();
            while(true)
            {
                try
                {
                    foreach (var shard in shards)
                    {
                        _logger.LogInformation($"found {shards.Count} shards");
                        _logger.LogInformation($"start extracting from {shard.ConnectionString}");

                        try
                        {
                            await Reshard(shard);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.ToString());
                        }
                    }

                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                    _logger.LogError($"error resharding, retry");

                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            }
        }

        private async Task Reshard(Shard shard)
        {
            var messages = await GetMessagesToMoveAsync(shard);
            if (messages.Count == 0)
            {
                _logger.LogInformation($"found 0 nonconsistent rows on {shard.ConnectionString}");
                _logger.LogInformation($"{shard.ConnectionString} done");

                return;
            }
            _logger.LogInformation($"moving {messages.Count} nonconsistent rows from {shard.ConnectionString}");

            foreach (var message in messages)
            {
                await SaveAsync(message);
            }

            _logger.LogInformation($"deleting {messages.Count} nonconsistent rows from {shard.ConnectionString}");

            await DeleteMessagesToMoveAsync(shard);

            _logger.LogInformation($"{shard.ConnectionString} done");
        }

        private async Task<List<Message>> GetMessagesToMoveAsync(Shard shard)
        {
            return await _mySql.GetListAsync(shard.ConnectionString,
                "Get_MessagesHashCodeNotIn", FromReader, new[] {
                new MySqlParameter("@min", shard.Min),
                new MySqlParameter("@max", shard.Max) });
        }

        private Task DeleteMessagesToMoveAsync(Shard shard)
        {
            return _mySql.ExecuteNonQueryAsync(shard.ConnectionString,
                "Delete_MessagesHashCodeNotIn", new[] {
                new MySqlParameter("@min", shard.Min),
                new MySqlParameter("@max", shard.Max) });
        }
    }
}
