using Homework.Users.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Users
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UsersApiController : ControllerBase
    {
        public async Task<IEnumerable<SearchUserDto>> Search(string serchText)
        {
            return Enumerable.Empty<SearchUserDto>();
        }
    }
}
