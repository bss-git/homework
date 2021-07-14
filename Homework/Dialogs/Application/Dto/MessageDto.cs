using System;

namespace Homework.Dialogs.Application.Dto
{
    internal class MessageDto
    {
        private Guid From { get; }

        private Guid To { get; }

        private string Message { get; }

        public MessageDto(Guid from, Guid to, string text, string fromName, string toName)
        {
            From = from;
            To = to;
            Message = text;
        }
    }
}