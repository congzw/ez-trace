using OpenTracing;

namespace EzTrace.DemoApp.FooDemo
{
    public class FooData
    {
        private readonly ITracer _tracer;

        public FooData(ITracer tracer)
        {
            _tracer = tracer;
        }

        public string GetUserInfo(string username)
        {
            using (var scope = _tracer.BuildSpan("FooData-GetUserInfo").StartActive(true))
            {
                //按需使用
                scope.Span.Log("a log from FooData");
                scope.Span.SetTag("username", username);

                var result = $"some info of {username}";
                return result;
            }
        }
    }
}