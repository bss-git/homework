using Dialogs.Application.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dialogs.Application
{
    public interface IDialogsRepository
    {
        Task SaveAsync(Message message);

        Task<IEnumerable<Message>> GetListAsync(Guid user1, Guid user2);
    }
}