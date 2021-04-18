using System;
using System.Threading.Tasks;

namespace Homework.Friends
{
    public interface IFriendLinkRepository
    {
        Task SaveAsync(Guid friend1, Guid friend2);
    }
}