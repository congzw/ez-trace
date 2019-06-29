using System;
using System.Collections.Generic;
using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger.Senders;
using Microsoft.Extensions.Logging;

namespace EzTrace.MySpans
{
    public class MockTracerFactory
    {
        public Dictionary<string, Tracer> CacheTracers { get; set; }

        public MockTracerFactory(string endPoint)
        {
            EndPoint = endPoint;
            CacheTracers = new Dictionary<string, Tracer>(StringComparer.OrdinalIgnoreCase);
        }

        public string EndPoint { get; set; }

        public Tracer GetTracer(string serviceName, ILoggerFactory loggerFactory)
        {
            if (CacheTracers.ContainsKey(serviceName))
            {
                return CacheTracers[serviceName];
            }

            var traceBuilder = new Tracer.Builder(serviceName)
                .WithSampler(new ConstSampler(true))
                .WithLoggerFactory(loggerFactory);

            //var loggerFactory = traceBuilder.LoggerFactory;
            var metrics = traceBuilder.Metrics;

            //try result: 16686:X 14268:OK
            var sender = new HttpSender(EndPoint);

            var reporter = new RemoteReporter.Builder()
                .WithLoggerFactory(loggerFactory)
                .WithMetrics(metrics)
                .WithSender(sender)
                .Build();

            var tracer = traceBuilder
                .WithReporter(reporter)
                .Build();

            CacheTracers.Add(serviceName, tracer);
            return tracer;
        }
    }
}
