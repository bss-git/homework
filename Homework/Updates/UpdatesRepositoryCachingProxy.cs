using Homework.Events;
using Homework.Events.RabbitMQ;
using Homework.Friends;
using Homework.Friends.Dto;
using Homework.Persistence;
using Homework.Updates.Dto;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Nito.Collections;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Homework.Updates
{
    public class UpdatesRepositoryCachingProxy : IUpdatesRepository
    {
        private readonly MySqlUpdatesRepository _repo;
        private readonly IFriendLinkRepository _friensRepo;
        private IMemoryCache _cache;
        private KafkaConsumer _kafkaConsumer;
        private KafkaProducer _kafkaProducer;
        private Channel<object> _changesQueue = Channel.CreateUnbounded<object>();

        public UpdatesRepositoryCachingProxy(MySqlUpdatesRepository repository, IFriendLinkRepository friensRepo,
            IMemoryCache cache, KafkaConsumer kafkaConsumer, KafkaProducer kafkaProducer)
        {
            _repo = repository;
            _friensRepo = friensRepo;
            _cache = cache;
            _kafkaConsumer = kafkaConsumer;
            _kafkaProducer = kafkaProducer;

            Task.Run(CacheUpdateTask);
            Task.Run(() => _kafkaConsumer.ConsumeAsync<UpdateViewModel>("updates",
                update => _changesQueue.Writer.WriteAsync(update), CancellationToken.None));
            Task.Run(() => _kafkaConsumer.ConsumeAsync<FriendLinkEvent>("friendLinkEvents",
                friendLinkEvent => _changesQueue.Writer.WriteAsync(friendLinkEvent), CancellationToken.None));
        }

        public Task<IEnumerable<UpdateViewModel>> GetListAsync(Guid userId)
        {
            if (_cache.TryGetValue<IEnumerable<UpdateViewModel>>(userId, out var result))
                return Task.FromResult(result);

            return GetListFromDbAsync(userId);
        }

        public async Task SaveAsync(UpdateViewModel update)
        {
            await _repo.SaveAsync(update);
            _ = _kafkaProducer.ProduceAsync("updates", update);
        }

        private async Task CacheUpdateTask()
        {
            await foreach (var change in _changesQueue.Reader.ReadAllAsync())
            {
                if (change is UpdateViewModel update)
                {
                    await HandleNewUpdateEvent(update);
                }
                if (change is FriendLinkEvent friendLinkEvent)
                {
                    await HandleFriendLinkChange(friendLinkEvent);
                }
            }
        }

        private async Task<IEnumerable<UpdateViewModel>> GetListFromDbAsync(Guid userId)
        {
            var result = await _repo.GetListAsync(userId);

            _cache.Set(userId, new Deque<UpdateViewModel>(result), new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });

            return result;
        }

        private async Task HandleNewUpdateEvent(UpdateViewModel update)
        {
            var friends = await _friensRepo.GetFriendIdsAsync(update.UserId);

            foreach (var friend in friends.Prepend(update.UserId))
            {
                if (_cache.TryGetValue<Deque<UpdateViewModel>>(friend, out var updates))
                {
                    updates.AddToFront(update);
                    if (updates.Count > 1000)
                        updates.RemoveFromBack();
                }
            }
        }

        private async Task HandleFriendLinkChange(FriendLinkEvent friendLinkEvent)
        {
            if (friendLinkEvent.Status != FriendStatus.Friend)
                return;

            if (_cache.TryGetValue<Deque<UpdateViewModel>>(friendLinkEvent.From, out _))
            {
                await GetListFromDbAsync(friendLinkEvent.From);
            }

            if (_cache.TryGetValue<Deque<UpdateViewModel>>(friendLinkEvent.To, out _))
            {
                await GetListFromDbAsync(friendLinkEvent.To);
            }
        }
    }
}
