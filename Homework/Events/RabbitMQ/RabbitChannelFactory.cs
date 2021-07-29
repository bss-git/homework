using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Events.RabbitMQ
{
    public class RabbitChannelFactory
    {
        private readonly IConnection _connection;

        public RabbitChannelFactory(IOptions<RabbitOptions> options)
        {
            var factory = new ConnectionFactory()
            { 
                HostName = options.Value.Hostname,
                //UserName = options.Value.User,
                //Password = options.Value.Password
            };

            //factory.DispatchConsumersAsync = true;
            _connection = factory.CreateConnection();
        }

        public IModel GetChannel()
        {
            return _connection.CreateModel();
        }
    }
}
