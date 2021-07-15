using Homework.Dialogs.Application.Dto;
using Microsoft.Extensions.Options;
using System;

namespace Homework.Dialogs.Persistense
{
    public class DialogsShardSelector
    {
        public DialogsShardSelector(IOptions<DialogsMySqlOptions> options)
        {

        }

        internal string GetConnectionString(Guid user1, Guid user2)
        {
            throw new NotImplementedException();
        }
    }
}