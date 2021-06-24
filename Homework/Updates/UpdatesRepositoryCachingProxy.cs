using Homework.Events;
using Homework.Friends;
using Homework.Updates.Dto;
using Microsoft.Extensions.Caching.Memory;
using Nito.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Homework.Updates
{
    public class UpdatesRepositoryCachingProxy : IUpdatesRepository
    {
        private readonly IUpdatesRepository _repo;
        private readonly IFriendLinkRepository _friensRepo;
        private IMemoryCache _cache;
        private KafkaConsumer _kafkaConsumer;
        private KafkaProducer _kafkaProducer;

        private Channel<UpdateViewModel> _updatesQueue = Channel.CreateUnbounded<UpdateViewModel>();

        public UpdatesRepositoryCachingProxy(IUpdatesRepository repository, IFriendLinkRepository friensRepo, IMemoryCache cache, KafkaConsumer kafkaConsumer, KafkaProducer kafkaProducer)
        {
            _repo = repository;
            _friensRepo = friensRepo;
            _cache = cache;
            _kafkaConsumer = kafkaConsumer;
            _kafkaProducer = kafkaProducer;
            Task.Run(CacheUpdateTask);
            _kafkaConsumer.ConsumeAsync<UpdateViewModel>("updates", update => _updatesQueue.Writer.WriteAsync(update), CancellationToken.None);
        }

        public async Task<IEnumerable<UpdateViewModel>> GetListAsync(Guid userId)
        {
            if (_cache.TryGetValue<IEnumerable<UpdateViewModel>>(userId, out var result))
                return result;

            result = await _repo.GetListAsync(userId);

            _cache.Set(userId, new Deque<UpdateViewModel>(result), new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(30)
            });

            return result;
        }

        public async Task SaveAsync(UpdateViewModel update)
        {
            await _repo.SaveAsync(update);
            _ = _kafkaProducer.ProduceAsync("updates", update);
        }

        private async Task CacheUpdateTask()
        {
            await foreach (var update in _updatesQueue.Reader.ReadAllAsync())
            {
                var friends = await _friensRepo.GetFriendIdsAsync(update.UserId);
                foreach(var friend in friends)
                {
                    if (_cache.TryGetValue<Deque<UpdateViewModel>>(friend, out var updates))
                    {
                        updates.AddToFront(update);
                        if (updates.Count > 1000)
                            updates.RemoveFromBack();
                    }
                }
            }
        }
    }
}
