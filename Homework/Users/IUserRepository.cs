using Homework.Models;
using Homework.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Users
{
    public interface IUserRepository
    {
        Task<User> GetAsync(string login);

        Task<User> GetAsync(Guid id);

        Task<IEnumerable<UserShortDto>> GetListAsync(int offset, int limit);

        Task<IEnumerable<UserShortDto>> GetFriendsAsync(Guid userId);

        Task<IEnumerable<UserShortDto>> GetOfferedFriendsAsync(Guid userId);

        Task SaveAsync(User person);
    }
}
