using Homework.Users;
using Homework.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Persistence.Tarantool
{
    public class TarantoolUserRepository : IUserRepository
    {
        private readonly MySqlUserRepository _mySqlRepo;
        private readonly TarantoolDb _tarantoolDb;

        public TarantoolUserRepository(MySqlUserRepository mySqlRepo)
        {
            _mySqlRepo = mySqlRepo;
        }

        public Task<User> GetAsync(string login)
        {
            //var tuple = _tarantoolDb.GetItemAsync<string, )>(login);
            return _mySqlRepo.GetAsync(login);
        }

        public Task<User> GetAsync(Guid id)
        {
            return _mySqlRepo.GetAsync(id);
        }

        public Task<IEnumerable<UserShortDto>> GetFriendsAsync(Guid userId)
        {
            return _mySqlRepo.GetFriendsAsync(userId);
        }

        public Task<IEnumerable<UserShortDto>> GetListAsync(int offset, int limit)
        {
            return _mySqlRepo.GetListAsync(offset, limit);
        }

        public Task<IEnumerable<UserShortDto>> GetOfferedFriendsAsync(Guid userId)
        {
            return _mySqlRepo.GetOfferedFriendsAsync(userId);
        }

        public Task SaveAsync(User person)
        {
            return _mySqlRepo.SaveAsync(person);
        }

        public Task<IEnumerable<UserShortDto>> SearchAsync(string name, string surname)
        {
            return _mySqlRepo.SearchAsync(name, surname);
        }
    }
}
