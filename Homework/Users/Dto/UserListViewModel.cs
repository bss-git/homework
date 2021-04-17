using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Users.Dto
{
    public class UserListViewModel
    {
        public Guid Id { get; }

        public string Login { get; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string City { get; set; }

        public UserListViewModel() { }

        public UserListViewModel(User user)
        {
            Id = user.Id;
            Login = user.Login;
            Name = user.Name;
            Surname = user.Surname;
            City = user.City;
        }
    }
}
