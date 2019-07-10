using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TraceServer.Domain.Traces;

namespace TraceServer.Hubs
{
    public class TraceHub : Hub
    {
        private readonly IClientTracerBridge _clientTracerBridge;

        public TraceHub(IClientTracerBridge clientTracerBridge)
        {
            _clientTracerBridge = InitCallback(clientTracerBridge);
        }

        public async Task StartSpan(StartSpanArgs args)
        {
            await _clientTracerBridge.StartSpan(args);
        }

        public async Task Log(LogArgs args)
        {
            await _clientTracerBridge.Log(args);
        }

        public async Task SetTags(SetTagArgs args)
        {
            await _clientTracerBridge.SetTags(args);
        }

        public async Task FinishSpan(FinishSpanArgs args)
        {
            await _clientTracerBridge.FinishSpan(args);
        }

        protected virtual IClientTracerBridge InitCallback(IClientTracerBridge bridge)
        {
            bridge.StartSpanCallback = result => Callback(nameof(bridge.StartSpanCallback), result);
            bridge.LogCallback = result => Callback(nameof(bridge.LogCallback), result);
            bridge.SetTagsCallback = result => Callback(nameof(bridge.SetTagsCallback), result);
            bridge.FinishSpanCallback = result => Callback(nameof(bridge.FinishSpanCallback), result);
            return bridge;
        }

        private async Task Callback(string method, TraceCallbackResult result)
        {
            await Clients.Caller.SendAsync(method, result);
        }
    }
}
