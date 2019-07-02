using System;
using System.Collections.Generic;

namespace EzTrace.OpenTracingConcepts
{
    public class Span
    {
        public Span()
        {
            Tags = new List<SpanTag>();
            Logs = new List<SpanLog>();
        }

        public string Operation { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime? FinishUtc { get; set; }
        public IList<SpanTag> Tags { get; set; }
        public IList<SpanLog> Logs { get; set; }

        public SpanContext Context { get; set; }
    }

    public class SpanTag
    {
        public SpanTag()
        {
            Items = new List<KeyValuePair<string, object>>();
        }
        public IList<KeyValuePair<string, object>> Items { get; set; }
    }

    public class SpanLog
    {
        public SpanLog()
        {
            Items = new List<KeyValuePair<string, object>>();
        }
        public DateTime TimestampUtc { get; set; }
        public string Message { get; set; }
        public IList<KeyValuePair<string, object>> Items { get; set; }
    }

    public class SpanContext
    {
        public SpanContext()
        {
            BaggageItems = new Dictionary<string, object>();
        }
        public IDictionary<string, object> BaggageItems { get; set; }
    }
}
