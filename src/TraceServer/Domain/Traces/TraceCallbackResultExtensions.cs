using System;
using System.Threading.Tasks;

namespace TraceServer.Domain.Traces
{
    public static class TraceCallbackResultExtensions
    {
        public static async Task SafeInvoke(this TraceCallbackResult result, Func<TraceCallbackResult, Task> callback)
        {
            if (callback == null)
            {
                return;
            }
            await callback(result);
        }
    }
}