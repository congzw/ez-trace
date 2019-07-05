using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TraceServer.Domain.Traces
{
    public interface IClientTracerFactory
    {
        IClientTracer GetTracer(string traceId);
    }

    public class ClientTracerFactory : IClientTracerFactory
    {
        public IDictionary<string, IClientTracer> ClientTracers { get; set; }

        public ClientTracerFactory()
        {
            ClientTracers = new ConcurrentDictionary<string, IClientTracer>(StringComparer.OrdinalIgnoreCase);
        }

        public IClientTracer GetTracer(string traceId)
        {
            var tryFixTraceId = TryFixTraceId(traceId);
            if (!ClientTracers.ContainsKey(tryFixTraceId))
            {
                ClientTracers[tryFixTraceId] = CreateTracer(tryFixTraceId);
            }
            return ClientTracers[tryFixTraceId];
        }
        
        private IClientTracer CreateTracer(string traceId)
        {
            return new MockClientTracer(traceId);
        }

        private string TryFixTraceId(string traceId)
        {
            return traceId ?? DefaultTracerId;
        }

        public static string DefaultTracerId = "DefaultTracer";
    }
}