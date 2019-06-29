using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jaeger;
using Microsoft.Extensions.Logging;

namespace EzTrace.MySpans
{
    public interface IMySpanRecorder
    {
        Task RecordSpanAsync(Span span);
        void Flush();
    }

    public class MySpanRecorder : IMySpanRecorder
    {
        public Task RecordSpanAsync(Span span)
        {
            var utcNow = span.Tracer.Clock.UtcNow();
            return Task.Run(() =>
            {
                var temp = Convert.ConvertToTempSpan(span, utcNow);
                TempSpans.Enqueue(temp);
            });
        }
        
        public Func<bool> ShouldRecording = () => true;

        private readonly ILogger _logHelper;
        private readonly Task _flushTask;
        public TimeSpan FlushInterval { get; set; }
        public ConcurrentQueue<TempSpan> TempSpans { get; set; }
        public MySpanConvert Convert { get; set; }
        public IMySpanStorage Storage { get; set; }

        public MySpanRecorder(IMySpanStorage storage, TimeSpan flushInterval, MySpanConvert convert, ILogger logger)
        {
            Storage = storage;
            _logHelper = logger;
            FlushInterval = flushInterval;
            Convert = convert;
            TempSpans = new ConcurrentQueue<TempSpan>();
            _flushTask = Task.Factory.StartNew(FlushLoop, TaskCreationOptions.LongRunning);
        }

        private void SaveTempSpans(IList<TempSpan> tempSpans)
        {
            if (tempSpans == null || tempSpans.Count == 0)
            {
                return;
            }

            try
            {
                var filePath = Storage.AutoCreateFilePath();
                var myRecord = Convert.ConvertToMyRecord(tempSpans);
                Storage.AddOrUpdate(filePath, myRecord);
            }
            catch (Exception ex)
            {
                _logHelper.Log(LogLevel.Error, ex.Message);
            }
        }

        public void Flush()
        {
            var tempSpans = new List<TempSpan>();
            do
            {
                TempSpans.TryDequeue(out var result);
                if (result != null)
                {
                    tempSpans.Add(result);
                }
            }
            while (!TempSpans.IsEmpty);

            if (tempSpans.Count > 0)
            {
                SaveTempSpans(tempSpans);
            }
        }

        private async void FlushLoop()
        {
            while (ShouldRecording())
            {
                // First flush should happen later so we start with the delay
                await Task.Delay(FlushInterval).ConfigureAwait(false);
                Flush();
            }
            Flush();
        }
    }
}
