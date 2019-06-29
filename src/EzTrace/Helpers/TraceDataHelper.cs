using System.Collections.Generic;
using EzTrace.MySpans;
using Jaeger;

namespace EzTrace.Helpers
{
    public class MyTraceHelper
    {
        public MyTrace PrepareMyTrace(string endPoint)
        {
            var spanLoader = MyFactory.GetMySpanLoader();
            var myRecords = spanLoader.GetMyRecords();

            var mockTracerFactory = MyFactory.GetMockTracerFactory(endPoint);
            var convert = MyFactory.CreateMySpanConvert();
            var loggerFactory = MyFactory.GetLoggerFactory();

            var myTrace = new MyTrace();
            foreach (var myRecord in myRecords)
            {
                var tempSpans = convert.ConvertBackToTempSpans(myRecord);
                var spanDic = convert.ConvertBackToSpanDic(tempSpans, mockTracerFactory, loggerFactory);
                foreach (var spanItem in spanDic)
                {
                    myTrace.SpanDic.Add(spanItem.Key, spanItem.Value);
                }
            }

            return myTrace;
        }
        
        public void LoadMyTrace(MyTrace myTrace)
        {
            foreach (var spanItem in myTrace.SpanDic)
            {
                var tempSpan = spanItem.Key;
                var span = spanItem.Value;
                //report to collector to reshow trace record
                span.Finish(tempSpan.Span.StopTime);
            }
        }
    }

    public class MyTrace
    {
        public MyTrace()
        {
            SpanDic = new Dictionary<TempSpan, Span>();
        }

        public IDictionary<TempSpan, Span> SpanDic { get; set; }
    }
}
