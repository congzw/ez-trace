using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using EzTrace.Common;
using Jaeger;
using Microsoft.Extensions.Logging;

namespace EzTrace.MySpans
{
    public class MySpanConvert
    {
        public Assembly SpanAssembly { get; set; }

        public TempSpan ConvertToTempSpan(Span span, DateTime stopUtc)
        {
            var myLocalSpan = new MySpan();

            myLocalSpan.TraceId = span.Context.TraceId.ToString();
            myLocalSpan.SpanId = span.Context.SpanId.ToString();
            myLocalSpan.Flags = (int)span.Context.Flags;
            myLocalSpan.OpName = span.OperationName;

            var references = span.GetReferences();
            foreach (var reference in references)
            {
                var myLocalReference = new MyLocalReference();
                myLocalReference.Type = reference.Type;
                if (reference.Context != null)
                {
                    myLocalReference.SpanID = reference.Context.SpanId.ToString();
                    myLocalReference.TraceID = reference.Context.TraceId.ToString();
                }
                myLocalSpan.References.Add(myLocalReference);
            }

            myLocalSpan.StartTime = span.StartTimestampUtc;
            //public int Duration { get; set; }
            myLocalSpan.StopTime = stopUtc;

            var tags = span.GetTags();
            foreach (var tag in tags)
            {
                myLocalSpan.Tags.Add(tag.Key, tag.Value);
            }

            var logs = span.GetLogs();
            foreach (var log in logs)
            {
                myLocalSpan.Logs.Add(MyLogData.Create(log));
            }

            var myProcess = new MyProcess();
            myProcess.ServiceName = span.Tracer.ServiceName;
            foreach (var keyValuePair in span.Tracer.Tags)
            {
                myProcess.Tags.Add(keyValuePair.Key, keyValuePair.Value);
            }

            myLocalSpan.ProcessKey = myProcess.CreateKey();

            var temp = new TempSpan(myProcess, myLocalSpan);
            return temp;
        }

        public MyRecord ConvertToMyRecord(IList<TempSpan> tempSpans)
        {
            var myRecord = new MyRecord();
            foreach (var tempSpan in tempSpans)
            {
                myRecord.Spans.Add(tempSpan.Span);
                var theProcess = myRecord.Processes.SingleOrDefault(x => x.CreateKey() == tempSpan.Process.CreateKey());
                if (theProcess == null)
                {
                    myRecord.Processes.Add(tempSpan.Process);
                }
            }

            return myRecord;
        }

        public IList<TempSpan> ConvertBackToTempSpans(MyRecord myRecord)
        {
            var tempSpans = new List<TempSpan>();
            if (myRecord == null)
            {
                return tempSpans;
            }

            var processes = myRecord.Processes;
            var mySpans = myRecord.Spans;

            foreach (var mySpan in mySpans)
            {
                var process = processes.Single(x => x.CreateKey() == mySpan.ProcessKey);
                var tempSpan = new TempSpan(process, mySpan);
                tempSpans.Add(tempSpan);
            }

            return tempSpans;
        }

        public IDictionary<TempSpan, Span> ConvertBackToSpanDic(IList<TempSpan> tempSpans,
            MockTracerFactory mockTracerFactory, ILoggerFactory loggerFactory)
        {
            var spanDic = new Dictionary<TempSpan, Span>();
            foreach (var tempSpan in tempSpans)
            {
                var tracer = mockTracerFactory.GetTracer(tempSpan.Process.ServiceName, loggerFactory);
                var span = ConvertToSpan(tempSpan, tracer);
                spanDic.Add(tempSpan, span);
            }
            return spanDic;
        }

        private Span ConvertToSpan(TempSpan tempSpan, Tracer tracer)
        {
            var mySpan = tempSpan.Span;
            var tags = mySpan.Tags;

            var parentId = TryFindParentId(mySpan);
            var context = new SpanContext(TraceId.FromString(mySpan.TraceId),
                SpanId.FromString(mySpan.SpanId),
                parentId,
                (SpanContextFlags)mySpan.Flags);

            if (SpanAssembly == null)
            {
                var filePath = AppDomain.CurrentDomain.Combine("Jaeger.dll");
                if (!File.Exists(filePath))
                {
                    throw new InvalidOperationException("SpanAssembly Not Found: " + filePath);
                }
                SpanAssembly = Assembly.LoadFile(filePath);
            }

            var references = new List<Reference>();
            foreach (var reference in mySpan.References)
            {
                references.Add(new Reference(context, reference.Type));
            }

            //hack it, attention for same types, or fail wit ex: System.MissingMethodException!
            //internal Span(
            //    Tracer tracer,
            //    string operationName,
            //    SpanContext context,
            //    DateTime startTimestampUtc,
            //    Dictionary<string, object> tags,
            //    IReadOnlyList<Reference> references)

            var span = CreateSpanWithReflection(SpanAssembly,
                tracer,
                mySpan.OpName,
                context,
                mySpan.StartTime.ToUniversalTime(),
                tags,
                references.AsReadOnly()
                );


            var logs = mySpan.Logs;
            foreach (var log in logs)
            {
                if (log.Message != null)
                {
                    //"fields": [
                    //{
                    //    "key": "event",
                    //    "type": "string",
                    //    "value": "myLog by FooDomain"
                    //}
                    span.Log(log.TimestampUtc, log.Message);
                }
                else
                {
                    span.Log(log.TimestampUtc, log.Fields);
                }
            }

            //span.Finish(mySpan.StopTime);
            return span;
        }

        private SpanId TryFindParentId(MySpan mySpan)
        {
            var myLocalReferences = mySpan.References;
            if (myLocalReferences != null)
            {
                foreach (var reference in myLocalReferences)
                {
                    //reference.Type may be lower case!
                    if ("CHILD_OF".Equals(reference.Type, StringComparison.OrdinalIgnoreCase))
                    {
                        return SpanId.FromString(reference.SpanID);
                    }
                }
            }
            return new SpanId();
        }

        private Span CreateSpanWithReflection(Assembly assembly, params object[] args)
        {
            var instance = assembly.CreateInstance(
                "Jaeger.Span",
                true,
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                args,
                null,
                null);
            return (Span)instance;

        }
    }
}