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

        public string Name { get; private set; }

        public string Surname { get; private set; }

        public DateTime BirthDate { get; private set; }

        public Sex Sex { get; private set; }

        public string Interest { get; private set; }

        public string City { get; private set; }

        public User(Guid id, string login)
        {
            Id = id;
            Login = login;
        }
    }
}
