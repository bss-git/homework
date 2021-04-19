using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Homework.Friends
{
    public interface IFriendLinkRepository
    {
        Task SaveAsync(Guid friend1, Guid friend2);

        Task<IEnumerable<Guid>> GetFriendIdsAsync(Guid userId);
    }
}