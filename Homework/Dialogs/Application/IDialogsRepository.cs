using Homework.Dialogs.Application.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Homework.Dialogs.Application
{
    internal interface IDialogsRepository
    {
        public Task Save(Guid from, Guid to, string text);

        public Task<IEnumerable<MessageDto>> GetList(Guid user1, Guid user2);
    }
}