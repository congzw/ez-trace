using System.Threading;
using System.Threading.Tasks;
using Jaeger;
using Jaeger.Reporters;

namespace EzTrace.MySpans
{
    public class MySpanReporter : IReporter
    {
        public IMySpanRecorder Recorder { get; set; }

        public MySpanReporter(IMySpanRecorder mySpanWriter)
        {
            Recorder = mySpanWriter;
        }
        public void Report(Span span)
        {
            //async
            Recorder.RecordSpanAsync(span);
        }

        public Task CloseAsync(CancellationToken cancellationToken)
        {
            //flush any way?
            //_logHelper.Info(@"!!! MyLocalReporter CloseAsync ");
            return Task.FromResult(0);
        }
    }
}
