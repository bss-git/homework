using Homework.Friends;
using Homework.Updates.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Updates
{
    public class UpdateMessageDispatchingDecorator : IUpdatesRepository
    {
        private readonly UpdatesMessageBus _messageBus;

        private readonly IFriendLinkRepository _friensRepo;

        private readonly IUpdatesRepository _decorated;

        public UpdateMessageDispatchingDecorator(UpdatesRepositoryCachingProxy decorated, UpdatesMessageBus messageBus, IFriendLinkRepository friensRepo)
        {
            _messageBus = messageBus;
            _friensRepo = friensRepo;
            _decorated = decorated;
        }

        public Task<IEnumerable<UpdateViewModel>> GetListAsync(Guid userId)
        {
            return _decorated.GetListAsync(userId);
        }

        public async Task SaveAsync(UpdateViewModel update)
        {
            var friends = await _friensRepo.GetFriendIdsAsync(update.UserId);

            foreach (var friend in friends.Prepend(update.UserId))
            {
                _messageBus.Publish(new UpdateMessage { Recepient = friend, Update = update });
            }

            await _decorated.SaveAsync(update);
        }
    }
}
