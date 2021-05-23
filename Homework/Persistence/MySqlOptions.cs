﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Persistence
{
    public class MySqlOptions
    {
        public string MasterHost { get; set; }

        public string ReadReplicas{ get; set; }

        public string Database { get; set; }

        public string User { get; set; }

        public string Password { get; set; }
    }
}
