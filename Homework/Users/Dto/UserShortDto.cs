using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Users.Dto
{
    public class UserShortDto
    {
        public Guid Id { get; set;  }

        public string Login { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string City { get; set; }
    }
}
