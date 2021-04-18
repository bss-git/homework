using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Friends
{
    public class FriendOffer
    {
        public Guid Id { get; }

        public Guid From { get; }

        public Guid To { get; }

        public FriendOffer(Guid id, Guid from, Guid to)
        {
            Id = id;
            From = from;
            To = to;
        }
    }
}
