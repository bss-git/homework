using System;

namespace Dialogs.Application.Dto
{
    public class PostMessageCommand
    {
        public Guid To { get; set; }

        public string Text { get; set; }
    }
}