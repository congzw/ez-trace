using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TraceServer.Domain.Traces
{
    public interface IClientTracer
    {
        string TraceId { get; set; }

        Task<ClientSpan> StartSpan(StartSpanArgs args);

        Task<ClientSpan> GetSpan(IClientSpanLocate args);
    }

    public class MockClientTracer : IClientTracer
    {
        public MockClientTracer(string traceId)
        {
            TraceId = traceId;
            ClientSpans = new ConcurrentDictionary<string, ClientSpan>(StringComparer.OrdinalIgnoreCase);
        }

        public IDictionary<string, ClientSpan> ClientSpans { get; set; }

        public string TraceId { get; set; }

        public Task<ClientSpan> StartSpan(StartSpanArgs args)
        {
            //todo with jaeger tracer, get an span id
            var clientSpan = ClientSpan.Create(args.TraceId, args.ParentSpanId, Guid.NewGuid().ToString());
            ClientSpans.Add(clientSpan.SpanId, clientSpan);
            return Task.FromResult(clientSpan);
        }

        public Task<ClientSpan> GetSpan(IClientSpanLocate args)
        {
            ClientSpan clientSpan = null;
            if (ClientSpans.ContainsKey(args.SpanId))
            {
                clientSpan = ClientSpans[args.SpanId];
            }
            return Task.FromResult(clientSpan);
        }
    }

    public class GetSpanArgs
    {
        public string TraceId { get; set; }
        public string ParentSpanId { get; set; }
        public string SpanId { get; set; }
    }

    public class StartSpanArgs
    {
        public string TraceId { get; set; }
        public string ParentSpanId { get; set; }
        public string OpName { get; set; }
    }

    public class ClientSpan : IClientSpanLocate
    {
        public string TraceId { get; set; }
        public string ParentSpanId { get; set; }
        public string SpanId { get; set; }

        public void SetTags(IEnumerable<KeyValuePair<string,object>> tags)
        {
            //todo with jaeger
            foreach (var tag in tags)
            {
                Trace.WriteLine(string.Format("span {0} set tags: {1} {2}", this.ToLocateKey(), tag.Key, tag.Value));
            }
        }
        public void Log(IEnumerable<KeyValuePair<string, object>> logs)
        {
            //todo with jaeger
            foreach (var log in logs)
            {
                Trace.WriteLine(string.Format("span {0} set logs: {1} {2}", this.ToLocateKey(), log.Key, log.Value));
            }
        }
        public void Finish()
        {
            //todo with jaeger
            Trace.WriteLine(string.Format("span {0} finished", this.ToLocateKey()));
        }
        
        public static ClientSpan Create(IClientSpanLocate locate)
        {
            return new ClientSpan() { TraceId = locate.TraceId, ParentSpanId = locate.ParentSpanId, SpanId = locate.SpanId };
        }

        public static ClientSpan Create(string traceId, string parentSpanId, string spanId)
        {
            return new ClientSpan() { TraceId = traceId, ParentSpanId = parentSpanId, SpanId = spanId };
        }
    }

    public class LogArgs: IClientSpanLocate
    {
        public LogArgs()
        {
            Logs = new Dictionary<string, object>();
        }

        public string TraceId { get; set; }
        public string SpanId { get; set; }
        public string ParentSpanId { get; set; }

        public IDictionary<string, object> Logs { get; set; }
    }

    public class SetTagArgs : IClientSpanLocate
    {
        public SetTagArgs()
        {
            Tags = new Dictionary<string, object>();
        }

        public string TraceId { get; set; }
        public string SpanId { get; set; }
        public string ParentSpanId { get; set; }

        public IDictionary<string, object> Tags { get; set; }
    }

    public class FinishSpanArgs : IClientSpanLocate
    {
        public string TraceId { get; set; }
        public string SpanId { get; set; }
        public string ParentSpanId { get; set; }
    }

}
