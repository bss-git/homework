using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Users
{
    public class UserCreator
    {
        private readonly IUserRepository _userRepository;

        public UserCreator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> CreateAsync(string login)
        {
            var existing = await _userRepository.GetAsync(login);
            if (existing != null)
                throw new ExistingLoginException();

            var user = new User(Guid.NewGuid(), login);

            return user;
        }
    }
}
