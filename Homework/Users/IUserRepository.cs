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

        Task<IEnumerable<UserListViewModel>> GetListAsync(int offset, int limit);

        Task<IEnumerable<UserListViewModel>> GetFriendsAsync(Guid userId);

        Task<IEnumerable<UserListViewModel>> GetOfferedFriendsAsync(Guid userId);

        Task SaveAsync(User person);
    }
}
