using Microsoft.AspNetCore.Http;
using OpenTracing;
using OpenTracing.Propagation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracing
{
    public class RequestTracingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITracer _tracer;

        public RequestTracingMiddleware(RequestDelegate next, ITracer tracer)
        {
            _next = next;
            _tracer = tracer;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path.ToString().Contains("auth", StringComparison.OrdinalIgnoreCase))
                await _next(httpContext);
            else
                await DoTraceAsync(httpContext);
        }

        private async Task DoTraceAsync(HttpContext httpContext)
        {
            var originalBody = httpContext.Response.Body;
            await using var bodyStream = new MemoryStream();
            httpContext.Response.Body = bodyStream;

            var route = httpContext.Request.Path.Value?.ToLower();
            var operationName = $"{httpContext.Request.Method}::{route}";
            var httpHeadersCarrier = new TextMapExtractAdapter(httpContext.Request.Headers.ToDictionary(
                k => k.Key,
                v => v.Value.ToString()));

            var spanContext = _tracer.Extract(BuiltinFormats.HttpHeaders, httpHeadersCarrier);
            using var scope = _tracer.BuildSpan(operationName).AsChildOf(spanContext).StartActive(true);

            httpContext.Request.EnableBuffering();

            await LogRequestAsync(httpContext.Request, scope);
            await _next(httpContext);
            await LogResponseAsync(httpContext.Response, bodyStream, originalBody, scope);
        }

        private async Task LogRequestAsync(HttpRequest request, IScope scope)
        {
            try
            {
                using var requestReader = new StreamReader(request.Body, leaveOpen: true);
                var requestBody = await requestReader.ReadToEndAsync();
                request.Body.Seek(0, SeekOrigin.Begin);

                var logData = new Dictionary<string, object>
                {
                    {"Method", request.Method},
                    {"Uri", request.Path},
                    {"Query", request.QueryString},
                    {"Headers", request.Headers},
                    {"Content", requestBody}
                };

                scope.Span.Log(logData);
            }
            catch
            {
            }
        }

        private async Task LogResponseAsync(HttpResponse httpResponse, Stream bodyStream, Stream originalBodyStream, IScope scope)
        {
            try
            {
                bodyStream.Seek(0, SeekOrigin.Begin);
                using var streamReader = new StreamReader(bodyStream);
                var responseBody = await streamReader.ReadToEndAsync();
                bodyStream.Seek(0, SeekOrigin.Begin);

                await bodyStream.CopyToAsync(originalBodyStream);

                var logData = new Dictionary<string, object>
                {
                    {"StatusCode", httpResponse.StatusCode},
                    {"Headers", httpResponse.Headers},
                    {"Content", responseBody}
                };

                scope.Span.Log(logData);
            }
            catch
            {
            }
        }
    }
}
