using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Users.Dto
{
    public class UserListViewModel
    {
        public int CurrentOffset { get; set; }

        public int PageSize { get; set; }

        public IEnumerable<UserShortDto> Users{ get; set; }
    }
}
