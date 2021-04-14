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

        Task SaveAsync(User person);
    }
}
