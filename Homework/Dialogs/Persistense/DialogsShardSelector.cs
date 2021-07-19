using Homework.Dialogs.Application.Dto;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Homework.Dialogs.Persistense
{
    public class DialogsShardSelector
    {
        private List<Shard> _shards = new List<Shard>();

        public DialogsShardSelector(IOptions<DialogsMySqlOptions> options)
        {
            var shardStrings = options.Value.Shards.Split(',', StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < shardStrings.Length; i++)
            {
                var sb = new MySqlConnectionStringBuilder();
                sb.Server = shardStrings[i].Trim();
                sb.Pooling = true;
                sb.UserID = options.Value.User;
                sb.Password = options.Value.Password;
                sb.Database = options.Value.Database;

                var step = 1000 / (int) (shardStrings.Length);
                var min = step * i;
                var max = step * (i + 1) - 1;
                if (i == shardStrings.Length - 1)
                    max = 999;

                _shards.Add(new Shard(min, max, sb.ToString()));
            }
        }

        internal IReadOnlyList<Shard> GetShards()
        {
            return _shards;
        }

        internal string GetConnectionString(Guid user1, Guid user2)
        {
            return _shards.First(x => x.Contains(GetHashCode(user1, user2))).ConnectionString;
        }

        public int GetHashCode(Guid guid1, Guid guid2)
        {
            int hash = 173;
            var guids = new List<Guid> { guid1, guid2 }.OrderBy(x => x.ToString());
            foreach (Byte b in guids.SelectMany(x => x.ToByteArray()))
            {
                hash = hash * 983 + b;
            }

            return Math.Abs(hash) % 1000;
        }
    }
}