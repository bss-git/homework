using Homework.Updates.Dto;

namespace Homework.Updates.SignalR
{
    public interface IUpdatesSubscriber
    {
        public void HandleEvent(UpdateMessage updateEvent);
    }
}