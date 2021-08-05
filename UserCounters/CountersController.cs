using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserCounters.Domain;

namespace UserCounters.UserCounters
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountersController : ControllerBase
    {
        private readonly ICountersRepository _repo;

        public CountersController(ICountersRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCounters(Guid userId)
        {
            return Ok(await _repo.Get(userId));
        }
    }
}
