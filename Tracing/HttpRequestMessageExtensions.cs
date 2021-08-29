using OpenTracing.Propagation;
using OpenTracing.Util;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Tracing
{
    public static class HttpRequestMessageExtensions
    {
        public static void InjectTracing(this HttpRequestMessage request)
        {
            var tracer = GlobalTracer.Instance;
            if (tracer?.ActiveSpan?.Context != null)
            {
                var dictionary = new Dictionary<string, string>();
                tracer.Inject(tracer.ActiveSpan.Context, BuiltinFormats.HttpHeaders, new TextMapInjectAdapter(dictionary));

                foreach (var entry in dictionary)
                {
                    request.Headers.Add(entry.Key, entry.Value);
                }
            }
        }
    }
}
