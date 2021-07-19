using System;

namespace Homework.Dialogs.Application.Dto
{
    public class Message
    {
        public Guid Id { get; }

        public Guid From { get; }

        public Guid To { get; }

        public string Text { get; }

        public DateTime Timestamp { get; }

        public Message(Guid from, Guid to, string text, DateTime timestamp)
        {
            Id = Guid.NewGuid();
            From = from;
            To = to;
            Text = text;
            Timestamp = timestamp;
        }

        public Message(Guid id, Guid from, Guid to, string text, DateTime timestamp)
        {
            Id = id;
            From = from;
            To = to;
            Text = text;
            Timestamp = timestamp;
        }
    }
}