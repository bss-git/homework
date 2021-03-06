using Dialogs.Persistence.Kafka;
using Dialogs.Persistence.Mysql;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dialogs.Persistense
{
    public class OutboxEventSender : BackgroundService
    {
        private readonly MySqlDb _mySqlDb;
        private readonly DialogsShardSelector _shardSelector;
        private readonly KafkaProducer<string> _kafkaProducer;

        public OutboxEventSender(MySqlDb mySqlDb, DialogsShardSelector shardSelector, KafkaProducer<string> kafkaProducer)
        {
            _mySqlDb = mySqlDb;
            _shardSelector = shardSelector;
            _kafkaProducer = kafkaProducer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var shards = _shardSelector.GetShards();
                var tasks = new Task[shards.Count];
                for (var i = 0; i < shards.Count; i++)
                {
                    tasks[i] = SendingJob(shards[i].ConnectionString, stoppingToken);
                }

                await Task.WhenAll(tasks);
            }
            catch (OperationCanceledException)
            {
               
            }
        }

        private async Task SendingJob(string connectionString, CancellationToken stoppingToken)
        {
            try
            {
                var processing = new ConcurrentDictionary<string, object>();

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var outbox = await _mySqlDb.GetListAsync(connectionString, "Get_Outbox", FromReader);
                        foreach (var message in outbox.Where(x => !processing.ContainsKey(x.Id)))
                        {
                            _ = _kafkaProducer.ProduceAsync(message.Topic, message.Key, message.Value)
                                .ContinueWith(async t =>
                                {
                                    if (t.Status == TaskStatus.RanToCompletion)
                                        await _mySqlDb.ExecuteTextAsync(connectionString, $"DELETE FROM Outbox WHERE Id='{message.Id}'");
                                    else
                                        Console.WriteLine(t.Exception);

                                    processing.TryRemove(message.Id, out _);
                                });

                            processing.TryAdd(message.Id, null);
                        }

                        if (outbox.Count == 0)
                            await Task.Delay(1000);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error occured: {e}");
                        await Task.Delay(1000);
                    }
                }
            }
            catch (OperationCanceledException)
            {

            }
        }

        private (string Id, string Topic, string Key, string Value) FromReader(MySqlDataReader reader)
        {
            return (
                reader.GetString("Id"),
                reader.GetString("Topic"),
                reader.GetString("Key"),
                reader.GetString("Value")
            );
        }
    }
}
