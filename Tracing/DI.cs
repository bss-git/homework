using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger.Senders.Thrift;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Propagation;
using OpenTracing.Util;
using System;

namespace Tracing
{
    public static class DI
    {
        public static void AddJaegerTracing(this IServiceCollection services, JaegerConfig jaegerConfig)
        {
            services.AddSingleton<ITracer>(serviceProvider =>
            {
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>() ?? new LoggerFactory();
                return CreateTracer(loggerFactory, jaegerConfig);
            });
        }

        private static Tracer CreateTracer(ILoggerFactory loggerFactory, JaegerConfig jaegerConfig)
        {
            var sender = new UdpSender(jaegerConfig.Host, jaegerConfig.Port, 0);

            var reporter = new RemoteReporter.Builder()
                .WithLoggerFactory(loggerFactory)
                .WithSender(sender)
                .Build();

            var sampler = new ConstSampler(true);

            var tracer = new Tracer.Builder(jaegerConfig.ServiceName)
                .WithLoggerFactory(loggerFactory)
                .WithReporter(reporter)
                .WithSampler(sampler)
                .RegisterExtractor(BuiltinFormats.HttpHeaders, new HeadersExtractor())
                .RegisterInjector(BuiltinFormats.HttpHeaders, new HeadersInjector())
                .Build();

            GlobalTracer.Register(tracer);

            return tracer;
        }
    }
}
