using Homework.Events;
using Homework.Friends;
using Homework.Updates.Dto;
using SharedDto.UserCounters;
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
        private readonly KafkaProducer<string> _kafkaProducer;
        private readonly IUpdatesRepository _decorated;

        public UpdateMessageDispatchingDecorator(UpdatesRepositoryCachingProxy decorated, UpdatesMessageBus messageBus, IFriendLinkRepository friensRepo, KafkaProducer<string> kafkaProducer)
        {
            _messageBus = messageBus;
            _friensRepo = friensRepo;
            _kafkaProducer = kafkaProducer;
            _decorated = decorated;
        }

        public async Task<IEnumerable<UpdateViewModel>> GetListAsync(Guid userId)
        {
            var result =  await _decorated.GetListAsync(userId);
            _ = _kafkaProducer.ProduceAsync("user_update", Guid.NewGuid().ToString(), new UserCounterEvent { UserId = userId, EventType = EventType.UserRead }); ;

            return result;
        }

        public async Task SaveAsync(UpdateViewModel update)
        {
            var friends = await _friensRepo.GetFriendIdsAsync(update.UserId);

            await _decorated.SaveAsync(update);

            _messageBus.Publish(new UpdateMessage { Recepient = update.UserId, Update = update });

            foreach (var friend in friends)
            {
                _messageBus.Publish(new UpdateMessage { Recepient = friend, Update = update });
                _ = _kafkaProducer.ProduceAsync("user_update", Guid.NewGuid().ToString(), new UserCounterEvent { UserId = friend, EventType = EventType.NewMessage });
            }
        }
    }
}
