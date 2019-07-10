using System;
using System.Threading.Tasks;

namespace TraceServer.Domain.Traces
{
    public class ClientTracerBridge: IClientTracerBridge
    {
        private readonly IClientTracerFactory _factory;

        public ClientTracerBridge(IClientTracerFactory factory)
        {
            _factory = factory;
        }

        public Func<TraceCallbackResult, Task> StartSpanCallback { get; set; }

        public async Task StartSpan(StartSpanArgs args)
        {
            var clientTracer = _factory.GetTracer(args.TraceId);
            if (clientTracer != null)
            {
                var clientSpan = await clientTracer.StartSpan(args);
                if (clientSpan != null)
                {
                    await TraceCallbackResult.MethodResult(nameof(StartSpan), true, args).AutoSet(clientSpan)
                        .SafeInvoke(StartSpanCallback);
                    return;
                }
            }

            await TraceCallbackResult.MethodResult(nameof(StartSpan), false, args).SafeInvoke(StartSpanCallback);
        }

        public Func<TraceCallbackResult, Task> LogCallback { get; set; }

        public async Task Log(LogArgs args)
        {
            var clientTracer = _factory.GetTracer(args.TraceId);
            if (clientTracer != null)
            {
                var clientSpan = await clientTracer.GetSpan(args);
                if (clientSpan != null)
                {
                    clientSpan.Log(args.Logs);
                    await TraceCallbackResult.MethodResult(nameof(Log), true, args).SafeInvoke(LogCallback);
                    return;
                }
            }
            await TraceCallbackResult.MethodResult(nameof(Log), false, args).SafeInvoke(LogCallback);
        }

        public Func<TraceCallbackResult, Task> SetTagsCallback { get; set; }

        public async Task SetTags(SetTagArgs args)
        {
            var clientTracer = _factory.GetTracer(args.TraceId);
            if (clientTracer != null)
            {
                var clientSpan = await clientTracer.GetSpan(args);
                if (clientSpan != null)
                {
                    clientSpan.SetTags(args.Tags);
                    await TraceCallbackResult.MethodResult(nameof(SetTags), true, args).SafeInvoke(SetTagsCallback);
                    return;
                }
            }
            await TraceCallbackResult.MethodResult(nameof(SetTags), false, args).SafeInvoke(SetTagsCallback);
        }
        
        public Func<TraceCallbackResult, Task> FinishSpanCallback { get; set; }

        public async Task FinishSpan(FinishSpanArgs args)
        {
            var clientTracer = _factory.GetTracer(args.TraceId);
            if (clientTracer != null)
            {
                var clientSpan = await clientTracer.GetSpan(args);
                if (clientSpan != null)
                {
                    clientSpan.Finish();
                    await TraceCallbackResult.MethodResult(nameof(FinishSpan), true, args).SafeInvoke(FinishSpanCallback);
                    return;
                }
            }
            await TraceCallbackResult.MethodResult(nameof(FinishSpan), false, args).SafeInvoke(FinishSpanCallback);
        }
    }
}
