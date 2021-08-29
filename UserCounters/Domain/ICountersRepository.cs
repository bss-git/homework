using System;
using System.Threading.Tasks;

namespace UserCounters.Domain
{
    public interface ICountersRepository
    {
        Task<UserCounter> Get(Guid userId);
    }
}