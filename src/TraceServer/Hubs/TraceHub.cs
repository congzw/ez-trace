using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TraceServer.Domain.Traces;

namespace TraceServer.Hubs
{
    public class TraceHub : Hub
    {
        private readonly IClientTracerFactory _factory;

        public TraceHub(IClientTracerFactory factory)
        {
            _factory = factory;
        }

        public async Task StartSpan(StartSpanArgs args)
        {
            var clientTracer = _factory.GetTracer(args.TraceId);
            if (clientTracer != null)
            {
                var clientSpan = await clientTracer.StartSpan(args);
                if (clientSpan != null)
                {
                    await Clients.Caller.SendAsync("startSpanCallback", TraceCallbackResult.SuccessResult("StartSpan Success", args)
                        .AutoSet(clientSpan));
                    return;
                }
            }

            await Clients.Caller.SendAsync("startSpanCallback", TraceCallbackResult.FailResult("StartSpan Fail", args));
        }

        public async Task Log(LogArgs args)
        {
            var clientTracer = _factory.GetTracer(args.TraceId);
            if (clientTracer != null)
            {
                var clientSpan = await clientTracer.GetSpan(args);
                if (clientSpan != null)
                {
                    clientSpan.Log(args.Logs);
                    await Clients.Caller.SendAsync("logCallback", TraceCallbackResult.SuccessResult("Log Success", args));
                    return;
                }
            }
            
            await Clients.Caller.SendAsync("logCallback", TraceCallbackResult.FailResult("Log Fail", args));
        }

        public async Task SetTags(SetTagArgs args)
        {
            var clientTracer = _factory.GetTracer(args.TraceId);
            if (clientTracer != null)
            {
                var clientSpan = await clientTracer.GetSpan(args);
                if (clientSpan != null)
                {
                    clientSpan.SetTags(args.Tags);
                    await Clients.Caller.SendAsync("setTagsCallback", TraceCallbackResult.SuccessResult("SetTags Success", args));
                    return;
                }
            }

            await Clients.Caller.SendAsync("setTagsCallback", TraceCallbackResult.FailResult("SetTags Fail", args));
        }

        public async Task FinishSpan(FinishSpanArgs args)
        {
            var clientTracer = _factory.GetTracer(args.TraceId);
            if (clientTracer != null)
            {
                var clientSpan = await clientTracer.GetSpan(args);
                if (clientSpan != null)
                {
                    clientSpan.Finish();
                    await Clients.Caller.SendAsync("finishSpanCallback", TraceCallbackResult.SuccessResult("FinishSpan Success", args));
                    return;
                }
            }

            await Clients.Caller.SendAsync("finishSpanCallback", TraceCallbackResult.FailResult("FinishSpan Fail", args));
        }
    }
}
