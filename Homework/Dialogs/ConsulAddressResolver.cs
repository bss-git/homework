using Consul;
using Homework.ServiceDiscovery;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Homework.Dialogs
{
    public class ConsulAddressResolver
    {
        private string _address;
        private IConsulClient _client;
        private long _current;

        public ConsulAddressResolver(IOptions<ConsulOptions> options)
        {
            _address = options.Value.Address;
            _client = new ConsulClient(new ConsulClientConfiguration { Address = new Uri(_address) });
        }

        public async Task<string> GetAddress()
        {
            var allServices = await _client.Agent.Services();
            var dialogs = allServices.Response?
                .Where(s => s.Value.Service.Equals("dialogs", StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Value).ToList();

            var selected = GetRoundRobin(dialogs);

            return $"http://{selected.Address}:{selected.Port}";
        }

        private AgentService GetRoundRobin(List<AgentService> services)
        {
            Interlocked.Increment(ref _current);
            return services[(int) (_current % services.Count)];
        }
    }
}
