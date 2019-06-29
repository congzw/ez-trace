using System;
using System.Collections.Generic;
using System.Linq;
using Jaeger;

namespace EzTrace.MySpans
{
    //todo: short name for smaller json content size
    public class MyRecord
    {
        public MyRecord()
        {
            Processes = new List<MyProcess>();
            Spans = new List<MySpan>();
        }

        public IList<MySpan> Spans { get; set; }
        public IList<MyProcess> Processes { get; set; }
    }
    
    public class MySpan
    {
        public MySpan()
        {
            Logs = new List<MyLogData>();
            Tags = new Dictionary<string, object>();
            References = new List<MyLocalReference>();
        }
        public string TraceId { get; set; }
        public string SpanId { get; set; }
        public int Flags { get; set; }
        public string OpName { get; set; }
        public IList<MyLocalReference> References { get; set; }
        public DateTime StartTime { get; set; }
        //public int Duration { get; set; }
        public DateTime StopTime { get; set; }
        public Dictionary<string, object> Tags { get; set; }
        public IList<MyLogData> Logs { get; set; }

        public string ProcessKey { get; set; }
    }

    public class MyLogData
    {
        public MyLogData()
        {
            Fields =new List<KeyValuePair<string, object>>();
        }
        public DateTime TimestampUtc { get; set; }
        public string Message { get; set; }
        public IList<KeyValuePair<string, object>> Fields { get; set; }

        public static MyLogData Create(LogData log)
        {
            var myLogData = new MyLogData()
                { Message = log.Message, TimestampUtc = log.TimestampUtc };
            if (log.Fields != null && log.Fields.Any())
            {
                myLogData.Fields = log.Fields.ToList();
            }
            return myLogData;
        }

    }
    
    public class MyProcess
    {
        public MyProcess()
        {
            Tags = new Dictionary<string, object>();
        }
        public string ServiceName { get; set; }
        public Dictionary<string, object> Tags { get; set; }

        public string CreateKey()
        {
            if (Tags.ContainsKey(Constants.TracerHostnameTagKey))
            {
                return (ServiceName + "::" + Tags[Constants.TracerHostnameTagKey]).ToLower();
            }
            return ServiceName.ToLower();
        }
    }

    public class TempSpan
    {
        public TempSpan(MyProcess process, MySpan span)
        {
            Process = process;
            Span = span;
        }

        public MyProcess Process { get; set; }
        public MySpan Span { get; set; }
    }

    public class MyLocalReference
    {
        public string Type { get; set; }
        public string TraceID { get; set; }
        public string SpanID { get; set; }
    }
}
