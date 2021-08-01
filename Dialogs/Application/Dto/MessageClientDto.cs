using System;

namespace Dialogs.Application.Dto
{
    public class MessageClientDto
    {
        public Guid From { get; }

        public Guid To { get; }

        public string Text { get; }

        public DateTime Timestamp { get; }

        public bool IsMy { get; }

        public MessageClientDto(Message message, bool isMy)
        {
            From = message.From;
            To = message.To;
            Text = message.Text;
            IsMy = isMy;
            Timestamp = message.Timestamp;
        }
    }
}