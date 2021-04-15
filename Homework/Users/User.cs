using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Users
{
    public class User
    {
        public Guid Id { get; }

        public string Login { get; }

        public string Name { get; set; } = string.Empty;

        public string Surname { get; set; } = string.Empty;

        public DateTime BirthDate { get; set; }

        public Gender Gender { get; set; }

        public string Interest { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public User(Guid id, string login)
        {
            Id = id;
            Login = login;
        }
    }
}
