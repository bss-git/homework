using Homework.Updates.Dto;

namespace Homework.Updates
{
    public interface IUpdatesSubscriber
    {
        public void HandleEvent(UpdateMessage updateEvent);
    }
}