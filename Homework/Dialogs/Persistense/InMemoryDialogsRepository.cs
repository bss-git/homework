using Homework.Dialogs.Application;
using Homework.Dialogs.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Dialogs.Persistense
{
    public class InMemoryDialogsRepository : IDialogsRepository
    {
        private List<Message> _list = new List<Message>();

        public Task<IEnumerable<Message>> GetListAsync(Guid user1, Guid user2)
        {
            return Task.FromResult((IEnumerable<Message>)_list.Where(x => x.From == user1 && x.To == user2 || x.From == user2 && x.To == user1)
                .OrderBy(x => x.Timestamp));
        }

        public Task SaveAsync(Message message)
        {
            lock(_list)
            {
                _list.Add(message);
            }

            return Task.CompletedTask;
        }
    }
}
