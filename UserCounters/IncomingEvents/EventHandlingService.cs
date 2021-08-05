using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UserCounters.Persistence.Kafka;

namespace UserCounters.IncomingEvents
{
    public class EventHandlingService : BackgroundService
    {
        private readonly IOptions<KafkaOptions> _options;
        private readonly EventHandlerFactory _eventHandlerFactory;

        public EventHandlingService(IOptions<KafkaOptions> options, EventHandlerFactory eventHandlerFactory)
        {
            _options = options;
            _eventHandlerFactory = eventHandlerFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _options.Value.BootstrapServers,
                GroupId = "userCountersService",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true,
                EnableAutoOffsetStore = false
            };

            using var c = new ConsumerBuilder<string, string>(config).Build();
            
            while (true)
            {
                try
                {
                    c.Subscribe(new[] { "user_dialog", "user_update" });
                    break;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                    await Task.Delay(1000);
                }
            }

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumed = c.Consume(stoppingToken);

                        await _eventHandlerFactory.GetHandler(consumed).Handle(consumed);

                        c.StoreOffset(consumed);
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine(e);
                        Console.WriteLine($"Error occured: {e.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                c.Close();
            }
        }
    }
}
