using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Jaeger;

namespace TraceServer.Domain.Traces
{
    public class SpanHold
    {
        public SpanHold()
        {
            Spans = new ConcurrentDictionary<string,Tuple<string, ClientSpan, Span>>(StringComparer.OrdinalIgnoreCase);
        }

        public IDictionary<string, Tuple<string, ClientSpan, Span>> Spans { get; set; }

        public void Add(string spanId, ClientSpan clientSpan, Span span)
        {
            Spans.Add(spanId, Tuple.Create(spanId, clientSpan, span));
        }

        public Tuple<string, ClientSpan, Span> TryGet(string spanId)
        {
            Spans.TryGetValue(spanId, out var result);
            return result;
        }
    }
}
