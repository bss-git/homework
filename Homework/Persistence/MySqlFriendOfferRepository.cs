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
    public class MySqlFriendOfferRepository : IFriendOfferRepository
    {
        private MySqlDb _mySql;

        public MySqlFriendOfferRepository(MySqlDb mySqlDb)
        {
            _mySql = mySqlDb;
        }

        public Task SaveAsync(FriendOffer offer)
        {
            return _mySql.ExecuteNonQueryAsync("INSERT_FriendOffer", new[] {
                new MySqlParameter("@id", offer.Id.ToByteArray()),
                new MySqlParameter("@from", offer.From.ToByteArray()),
                new MySqlParameter("@to", offer.To.ToByteArray())
            });
        }

        public Task<FriendOffer> GetAsync(Guid from, Guid to)
        {
            return _mySql.GetItemAsync("GET_FriendOffer", FromReader, new[] {
                new MySqlParameter("@from", from.ToByteArray()),
                new MySqlParameter("@to", to.ToByteArray())});
        }

        private static FriendOffer FromReader(MySqlDataReader reader)
        {
            return new FriendOffer(reader.GetGuid("Id"), reader.GetGuid("from"), reader.GetGuid("to"));
        }

        public Task DeleteAync(Guid id)
        {
            return _mySql.ExecuteNonQueryAsync("DELETE_FriendOffer", new MySqlParameter("@id", id.ToByteArray()));
        }
    }
}
