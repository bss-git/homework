using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Friends
{
    public interface IFriendOfferRepository
    {
        Task SaveAsync(FriendOffer offer);

        Task<FriendOffer> GetAsync(Guid from, Guid to);

        Task DeleteAync(Guid id);
    }
}
