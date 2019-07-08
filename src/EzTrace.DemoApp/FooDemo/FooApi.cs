using OpenTracing;

namespace EzTrace.DemoApp.FooDemo
{
    public class FooApi
    {
        private readonly FooService _fooService;
        private readonly ITracer _tracer;

        public FooApi(FooService fooService, ITracer tracer)
        {
            _fooService = fooService;
            _tracer = tracer;
        }
        
        public string GetUserInfo(string username)
        {
            using (var scope = _tracer.BuildSpan("FooService-GetUserInfo").StartActive(true))
            {
                var result = _fooService.GetUserInfo(username);

                //按需使用
                scope.Span.Log("a log from FooApi");
                scope.Span.SetTag("username", username);

                return result;
            }
        }
    }
}