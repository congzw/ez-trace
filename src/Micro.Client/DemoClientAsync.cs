using System;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Util;

namespace Micro.Client
{
    public class DemoClientAsync
    {
        private readonly ITracer _tracer;
        private readonly ILogger<DemoClientAsync> _logger;

        public DemoClientAsync(ITracer tracer, ILoggerFactory loggerFactory)
        {
            _tracer = tracer;
            _logger = loggerFactory.CreateLogger<DemoClientAsync>();
        }

        public void CreateTraces()
        {
            var span1 = MockTraceSpan("op_1", 1, 4, null);
            var span1_1 = MockTraceSpan("op_1.1", 2, 3, span1);
            var span1_1_1 = MockTraceSpan("op_1.1.1", 5, 6, span1_1);
            Console.WriteLine("-------CreateTraces------");
        }

        public ISpan MockTraceSpan(string opName, int startSecond, int endSecond, ISpan parentSpan)
        {
            var now = DateTime.Now;
            var startTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, startSecond);
            if (parentSpan == null)
            {
                using (var scope = _tracer.BuildSpan(opName).WithStartTimestamp(startTime).StartActive(false))
                {
                    scope.Span.Log("log of " + opName);
                    scope.Span.Finish(startTime.AddSeconds(endSecond - startSecond));
                    return scope.Span;
                }
            }
            
            var localScopeManager = (AsyncLocalScopeManager)_tracer.ScopeManager;
            using (var parentScope = localScopeManager.Activate(parentSpan, false))
            {
                using (var scope = _tracer.BuildSpan(opName).WithStartTimestamp(startTime).StartActive(false))
                {
                    scope.Span.Log("log of " + opName);
                    scope.Span.Finish(startTime.AddSeconds(endSecond - startSecond));
                    return scope.Span;
                }
            }
        }

    }
}
