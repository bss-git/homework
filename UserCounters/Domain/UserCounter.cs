using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserCounters.Domain
{
    public class UserCounter
    {
        public Guid UserId { get; set; }

        public int Updates { get; set; }

        public int Dialogs { get; set; }
    }
}
