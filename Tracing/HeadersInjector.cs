using Jaeger;
using Jaeger.Propagation;
using OpenTracing.Propagation;

namespace Tracing
{
    public class HeadersInjector : Injector<ITextMap>
    {
        protected override void Inject(SpanContext spanContext, ITextMap carrier)
        {
            carrier.Set(TracingHeaders.TraceId, spanContext.TraceId.ToString());
            carrier.Set(TracingHeaders.SpanId, spanContext.SpanId.ToString());
            carrier.Set(TracingHeaders.ParentSpanId, spanContext.ParentId.ToString());
        }
    }
}
