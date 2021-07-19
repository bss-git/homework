using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Dialogs.Persistense
{
    public class Shard
    {
        public int Min { get; }

        public int Max { get; }

        public string ConnectionString { get; }

        public Shard(int min = 0, int max = 0, string connectionString = null)
        {
            Min = min;
            Max = max;
            ConnectionString = connectionString;
        }

        public bool Contains(int hashCode)
        {
            return hashCode >= Min && hashCode <= Max;
        }
    }
}
