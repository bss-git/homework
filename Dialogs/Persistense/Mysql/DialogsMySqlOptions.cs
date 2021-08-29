using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dialogs.Persistence.Mysql
{
    public class DialogsMySqlOptions
    {
        public string Shards { get; set; }

        public string FailedNodes { get; set; }

        public string Database { get; set; }

        public string User { get; set; }

        public string Password { get; set; }
    }
}
