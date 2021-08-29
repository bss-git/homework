using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Prometheus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dialogs
{
    public class RequestDurationMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            using (Metrics.RequestDuration.NewTimer())
            {
                Metrics.RequestsTotal.Inc();
                await next(context);
            }
        }
    }
}
