using Homework.Dialogs.Application.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Homework.Dialogs.Application
{
    public interface IDialogsRepository
    {
        Task SaveAsync(Message message);

        Task<string> GetListAsync(Guid user1, Guid user2);
    }
}