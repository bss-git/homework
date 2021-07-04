using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Friends.Dto
{
    public class FriendLinkEvent
    {
        public Guid From { get; set; }

        public Guid To{ get; set; }

        public FriendStatus Status { get; set; }
    }
}
