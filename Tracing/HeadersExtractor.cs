using Jaeger;
using Jaeger.Propagation;
using OpenTracing.Propagation;
using System;
using System.Linq;

namespace Tracing
{
    public class HeadersExtractor : Extractor<ITextMap>
    {
        protected override SpanContext Extract(ITextMap carrier)
        {
            var info = new string[3];
            foreach (var (key, value) in carrier)
            {
                if (key.Equals(TracingHeaders.TraceId, StringComparison.OrdinalIgnoreCase))
                {
                    info[0] = value;
                }
                else if (key.Equals(TracingHeaders.SpanId, StringComparison.OrdinalIgnoreCase))
                {
                    info[1] = value;
                }
                else if (key.Equals(TracingHeaders.ParentSpanId, StringComparison.OrdinalIgnoreCase))
                {
                    info[2] = value;
                }
            }

            if (info.Any(string.IsNullOrWhiteSpace))
                return null;

            return new SpanContext(TraceId.FromString(info[0]), SpanId.FromString(info[1]), SpanId.FromString(info[2]), SpanContextFlags.Sampled);
        }
    }
}
