using Homework.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Friends.Dto
{
    public class FriendsListViewModel
    {
        public IEnumerable<UserShortDto> Offers { get; set; }

        public IEnumerable<UserShortDto> Friends { get; set; }
    }
}
