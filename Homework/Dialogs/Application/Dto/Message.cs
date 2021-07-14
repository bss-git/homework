using System;

namespace Homework.Dialogs.Application.Dto
{
    public class Message
    {
        public Guid From { get; }

        public Guid To { get; }

        public string Text { get; }

        public DateTime Timestamp { get; }

        public Message(Guid from, Guid to, string text, DateTime timestamp)
        {
            From = from;
            To = to;
            Text = text;
            Timestamp = timestamp;
        }
    }
}