using System;
using System.Linq;
using System.Threading.Tasks;
using EzTrace.Common;
using EzTrace.DemoApp.FooDemo;
using Jaeger;
using OpenTracing;

namespace EzTrace.DemoApp.Helpers
{
    public class DemoHelper
    {
        private readonly ITracer _tracer;
        public int MockOpMilliseconds { get; set; }
        public DemoHelper(ITracer tracer)
        {
            _tracer = tracer;
            MockOpMilliseconds = 100;
        }

        public FooApi CreateFooApi()
        {
           return new FooApi(new FooService(new FooData(_tracer), _tracer), _tracer);
        }

        public void InvokeOp(string opName, int callDeep, params string[] nestedOps)
        {
            using (var theScope = _tracer.BuildSpan(opName).StartActive(true))
            {
                ScopeBegin(_tracer, opName, callDeep);

                var span = (Span)theScope.Span;
                span.Log("myLog by " + opName);
                span.SetTag("myTag", "tag of " + opName);
                span.SetBaggageItem("myBaggageItem", "baggageItem of " + opName);

                //mock a op times
                Task.Delay(TimeSpan.FromMilliseconds(MockOpMilliseconds + callDeep * 10)).Wait();

                //ShowActiveSpan(tracer, callDeep);

                if (nestedOps != null && nestedOps.Length > 0)
                {
                    InvokeOp(nestedOps[0], callDeep + 1, nestedOps.Skip(1).ToArray());
                }
            }
            ScopeEnd(_tracer, opName, callDeep);
        }

        private void ScopeBegin(ITracer tracer, string opName, int callDeep)
        {
            var spanId = TryGetSpanId(tracer);
            var traceId = TryGetTraceId(tracer);
            "<<< scope begin for op: {0} {1} {2}".WriteLineFormat(callDeep, opName, spanId, traceId);
        }
        private void ScopeEnd(ITracer tracer, string opName, int callDeep)
        {
            var spanId = TryGetSpanId(tracer);
            var traceId = TryGetTraceId(tracer);
            "scope end for op: {0} {1} {2} >>>".WriteLineFormat(callDeep, opName, spanId, traceId);
        }
        private void ShowActiveSpan(ITracer tracer, int callDeep)
        {
            var spanId = TryGetSpanId(tracer);
            "{0} => {1}".WriteLineFormat(callDeep, "ActiveSpan", spanId ?? "NULL");
        }
        private string TryGetSpanId(ITracer tracer)
        {
            return tracer.ActiveSpan?.Context.SpanId;
        }
        private string TryGetTraceId(ITracer tracer)
        {
            return tracer.ActiveSpan?.Context.TraceId;
        }
        private string TryGetSpanOpName(ITracer tracer)
        {
            return (tracer.ActiveSpan as Span)?.OperationName;
        }
    }
}
