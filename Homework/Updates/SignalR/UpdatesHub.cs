using Homework.Auth;
using Homework.Updates.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Homework.Updates.SignalR
{
    [Authorize]
    public class UpdatesHub : Hub
    {
        public UpdatesHub(UpdatesHubEventPublisher publisher)
        {
        }
    }
}
