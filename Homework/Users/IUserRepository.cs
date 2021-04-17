using Homework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Users
{
    public interface IUserRepository
    {
        Task<User> GetAsync(string login);

        Task<IEnumerable<User>> GetListAsync(int offset, int limit);

        Task SaveAsync(User person);
    }
}
