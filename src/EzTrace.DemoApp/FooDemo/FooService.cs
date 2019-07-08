using System.Threading.Tasks;
using OpenTracing;

namespace EzTrace.DemoApp.FooDemo
{
    public class FooService
    {
        private readonly FooData _fooData;
        private readonly ITracer _tracer;

        public FooService(FooData fooData, ITracer tracer)
        {
            _fooData = fooData;
            _tracer = tracer;
        }
        
        public string GetUserInfo(string username)
        {
            using (var scope = _tracer.BuildSpan("FooService-GetUserInfo").StartActive(true))
            {
                //按需使用
                scope.Span.Log("a log from FooService");
                scope.Span.SetTag("username", username);

                var result = _fooData.GetUserInfo(username);

                SomeWork();

                return result;
            }
        }


        public void SomeWork()
        {
            //will use "FooService-GetUserInfo" span
            _tracer.ActiveSpan.Log("another log from FooService's SomeWork");
            Task.Delay(100);
        }
    }
}
