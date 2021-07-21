using Homework.Updates.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Homework.Updates.SignalR
{
    public class UpdatesMessageBus
    {
        private List<IUpdatesSubscriber> _subs = new List<IUpdatesSubscriber>();

        private Channel<UpdateMessage> _events = Channel.CreateUnbounded<UpdateMessage>();

        public UpdatesMessageBus()
        {
            Task.Run(HanleEventsTask);
        }

        public void Subscribe(IUpdatesSubscriber subscriber)
        {
            lock (_subs)
            {
                //возможна утечка памяти, так как всегда держит ссылки на подписчиков
                //в реальности нужно использовать слабые ссылки
                _subs.Add(subscriber);
            }
        }

        public void Publish(UpdateMessage updateEvent)
        {
            _events.Writer.WriteAsync(updateEvent);
        }

        private async Task HanleEventsTask()
        {
            await foreach (var updateEvent in _events.Reader.ReadAllAsync())
            {
                foreach(var sub in _subs)
                {
                    sub.HandleEvent(updateEvent);
                }
            }
        }
    }
}
