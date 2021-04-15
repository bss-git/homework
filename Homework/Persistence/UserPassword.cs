using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Persistence
{
    public class UserPassword
    {
        public byte[] PasswordHash { get; set; }

        public string Salt { get; set; }
    }
}
