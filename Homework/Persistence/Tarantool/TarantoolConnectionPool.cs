using ProGaudi.Tarantool.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Homework.Persistence.Tarantool
{
    public class TarantoolConnectionPool
    {
        private const int _poolSize = 10;

        private Box[] _boxes = new Box[_poolSize];

        private long _currentBox;

        public TarantoolConnectionPool(string connectionString)
        {
            for(var i = 0; i < _poolSize; i++)
            {
                _boxes[i] = Box.Connect(connectionString).Result;
            }
        }

        public Box Box
        {
            get
            {
                Interlocked.Increment(ref _currentBox);

                return _boxes[_currentBox % _poolSize];
            }
        }
    }
}
