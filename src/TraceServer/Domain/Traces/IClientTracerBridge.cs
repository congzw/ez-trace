using System;
using System.Threading.Tasks;

namespace TraceServer.Domain.Traces
{
    public interface IClientTracerBridge
    {
        Func<TraceCallbackResult, Task> StartSpanCallback { get; set; }
        Task StartSpan(StartSpanArgs args);

        Func<TraceCallbackResult, Task> LogCallback { get; set; }
        Task Log(LogArgs args);

        Func<TraceCallbackResult, Task> SetTagsCallback { get; set; }
        Task SetTags(SetTagArgs args);

        Func<TraceCallbackResult, Task> FinishSpanCallback { get; set; }
        Task FinishSpan(FinishSpanArgs args);
    }
}