using Homework.Dialogs.Application.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Homework.Dialogs.Application
{
    public interface IDialogsRepository
    {
        Task SaveAsync(Guid from, Guid to, string text, DateTime timestamp);

        Task<IEnumerable<Message>> GetListAsync(Guid user1, Guid user2);
    }
}