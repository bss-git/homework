using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Updates.Dto
{
    public class UpdateMessage
    {
        public Guid Recepient { get; set; }

        public UpdateViewModel Update { get; set; }
    }
}
