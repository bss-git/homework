using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Events.RabbitMQ
{
    public class RabbitOptions
    {
        public string Hostname { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}
