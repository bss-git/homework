using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Users.Dto
{
    public class UserViewModel
    {
        public bool IsMe { get; set; }

        public User User { get; set; }
    }
}
