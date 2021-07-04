using Homework.Users;
using Homework.Users.Dto;
using ProGaudi.Tarantool.Client.Model;
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

        public TarantoolUserRepository(MySqlUserRepository mySqlRepo, TarantoolDb tarantoolDb)
        {
            _mySqlRepo = mySqlRepo;
            _tarantoolDb = tarantoolDb;
        }

        public async Task<User> GetAsync(string login)
        {
            var result = await _tarantoolDb.CallAsync<TarantoolTuple<string>, TarantoolTuple<string, byte[], string, string, DateTime, string, Gender, string>>
                ("get_user_by_login", TarantoolTuple.Create(login));

            if (!result.Any())
                return null;

            return UserMapping(result.First());
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

        public async Task SaveAsync(User person)
        {
            await _mySqlRepo.SaveAsync(person);
            await _tarantoolDb.CallAsync("insert_user", UserMapping(person));
        }

        public Task<IEnumerable<UserShortDto>> SearchAsync(string name, string surname)
        {
            return _mySqlRepo.SearchAsync(name, surname);
        }

        private static TarantoolTuple<string, byte[], string, string, DateTime, string, Gender, string> UserMapping(User user)
        {
            return TarantoolTuple.Create(user.Login, user.Id.ToByteArray(), user.Name, user.Surname, user.BirthDate, user.City, user.Gender, user.Interest);
        }

        private static User UserMapping(TarantoolTuple<string, byte[], string, string, DateTime, string, Gender, string> tuple)
        {
            return new User(new Guid(tuple.Item2), tuple.Item1)
            {
                Name = tuple.Item3,
                Surname = tuple.Item4,
                BirthDate = tuple.Item5,
                City = tuple.Item6,
                Gender = tuple.Item7,
                Interest = tuple.Item8
            };
        }
    }
}
