using Homework.Updates.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Updates
{
    public interface IUpdatesRepository
    {
        Task<IEnumerable<UpdateViewModel>> GetListAsync(Guid userId);

        Task SaveAsync(UpdateViewModel update);
    }
}
