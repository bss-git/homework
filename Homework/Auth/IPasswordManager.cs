using Homework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Auth
{
    public interface IPasswordManager
    {
        Task<bool> IsValidPasswordAsync(string login, string password);

        Task SavePasswordAsync(Guid userId, string password);
    }
}
