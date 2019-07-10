namespace TraceServer.Domain.Traces
{
    public class TraceCallbackResult : IClientSpanLocate
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        
        public string TraceId { get; set; }
        public string SpanId { get; set; }
        public string ParentSpanId { get; set; }
        
        public static TraceCallbackResult MethodResult(string method, bool success, object data = null)
        {
            var traceCallbackResult = new TraceCallbackResult() { Message = method + (success ? "Success" : "Fail"), Success = success, Data = data };
            traceCallbackResult.AutoSet(data as IClientSpanLocate);
            return traceCallbackResult;
        }

        public static TraceCallbackResult SuccessResult(string message, object data = null)
        {
            var traceCallbackResult = new TraceCallbackResult() {Message = message, Success = true, Data = data};
            traceCallbackResult.AutoSet(data as IClientSpanLocate);
            return traceCallbackResult;
        }

        public static TraceCallbackResult FailResult(string message, object data = null)
        {
            var traceCallbackResult = new TraceCallbackResult() { Message = message, Success = false, Data = data };
            traceCallbackResult.AutoSet(data as IClientSpanLocate);
            return traceCallbackResult;
        }

    }
}
