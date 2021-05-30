using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Updates.Dto
{
    public class UpdateViewModel
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public string Message { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
