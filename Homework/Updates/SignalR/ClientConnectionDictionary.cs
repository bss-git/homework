using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Updates.SignalR
{
    public class ClientConnectionDictionary
    {
        private ConcurrentDictionary<Guid, List<string>> _clients = new ConcurrentDictionary<Guid, List<string>>();

        public bool TryGetValue(Guid userId, out List<string> connections)
        {
            return _clients.TryGetValue(userId, out connections);
        }

        public void Add(Guid userId, string connection)
        {
            lock (_clients)
            {
                _clients.AddOrUpdate(userId, new List<string> { connection }, (u, c) => c.Append(connection).ToList());
            }
        }

        public void Remove(Guid userId, string connection)
        {
            if (!_clients.TryGetValue(userId, out var connections))
                return;

            lock (_clients)
            {
                connections.Remove(connection);
                if (!connections.Any())
                    _clients.TryRemove(userId, out _);
            }
        }
    }
}
