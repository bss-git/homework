using Auth;
using Homework.Auth;
using Homework.Users;
using Homework.Users.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Friends
{
    [Route("api/friends")]
    [ApiController]
    [Authorize]
    public class FriendsApiController : ControllerBase
    {
        private FriendManager _friendManager;
        private IUserRepository _userRepository;

        public FriendsApiController(FriendManager friendManager, IUserRepository userRepository)
        {
            _friendManager = friendManager;
            _userRepository = userRepository;

        }

        [HttpPost("offer")]
        public async Task<IActionResult> SendOffer([FromBody] Guid to)
        {
            if (to == Guid.Empty)
                return BadRequest("Некорректный идентификатор пользователя");

            var userTo = await _userRepository.GetAsync(to);
            if (userTo == null)
                return NotFound("Пользователь не найден");

            await _friendManager.SendOfferAsync(User.Id(), to);

            return Ok();
        }

        [HttpPost("accept")]
        public async Task<IActionResult> AcceptOffer([FromBody] Guid from)
        {
            if (from == Guid.Empty)
                return BadRequest("Некорректный идентификатор пользователя");

            var userTo = await _userRepository.GetAsync(from);
            if (userTo == null)
                return NotFound("Пользователь не найден");

            await _friendManager.AcceptOfferAsync(from, User.Id());

            return Ok();
        }

        [HttpGet("{friendId}/status")]
        public async Task<IActionResult> GetFriendStatus([FromRoute] Guid friendId)
        {
            if (friendId == Guid.Empty)
                return BadRequest("Некорректный идентификатор пользователя");

            var status = await _friendManager.GetFriendStatusAsync(User.Id(), friendId);

            return new JsonResult(new { status });
        }
    }
}
