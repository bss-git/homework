using Homework.Friends;
using Homework.Updates.Dto;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Homework.Updates
{
    public class UpdatesRepositoryCachingProxy : IUpdatesRepository
    {
        private readonly IUpdatesRepository _repo;
        private readonly IFriendLinkRepository _friensRepo;
        private IMemoryCache _cache;

        private Channel<Guid> _usersInvalidationQueue = Channel.CreateUnbounded<Guid>();

        public UpdatesRepositoryCachingProxy(IUpdatesRepository repository, IFriendLinkRepository friensRepo, IMemoryCache cache)
        {
            _repo = repository;
            _friensRepo = friensRepo;
            _cache = cache;
            Task.Run(CacheInvadidationTask);
        }

        public async Task<IEnumerable<UpdateViewModel>> GetListAsync(Guid userId)
        {
            if (_cache.TryGetValue<IEnumerable<UpdateViewModel>>(userId, out var result))
                return result;

            result = await _repo.GetListAsync(userId);

            _cache.Set(userId, result, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            }); ;

            return result;
        }

        public async Task SaveAsync(UpdateViewModel update)
        {
            await _repo.SaveAsync(update);
            var friends = await _friensRepo.GetFriendIdsAsync(update.UserId);
            await _usersInvalidationQueue.Writer.WriteAsync(update.UserId);
            foreach(var friend in friends)
            {
                await _usersInvalidationQueue.Writer.WriteAsync(friend);
            }
        }

        private async Task CacheInvadidationTask()
        {
            await foreach(var userToEvict in _usersInvalidationQueue.Reader.ReadAllAsync())
            {
                _cache.Remove(userToEvict);
            }
        }
    }
}
