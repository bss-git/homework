using Homework.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Friends.Dto
{
    public class FriendsListViewModel
    {
        public IEnumerable<UserListViewModel> Offers { get; set; }

        public IEnumerable<UserListViewModel> Friends { get; set; }
    }
}
