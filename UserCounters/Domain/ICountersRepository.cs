using System;
using System.Threading.Tasks;

namespace UserCounters.UserCounters
{
    public interface ICountersRepository
    {
        Task<UserCounter> Get(Guid userId);
    }
}