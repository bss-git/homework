using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserCounters.Domain
{
    public class UserCounterEvent
    {
        public Guid UserId { get; set; }

        public EventType EventType { get; set; }
    }
}
