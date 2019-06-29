using System;
using System.Reflection;
using EzTrace.Common;
using EzTrace.MySpans;
using Microsoft.Extensions.Logging;

namespace EzTrace.Helpers
{
    public class MyFactory
    {
        public static IJsonFileHelper CreateJsonFileHelper()
        {
            var jsonFileHelper = JsonFileHelper.Resolve();
            return jsonFileHelper;
        }

        private static MySpanStorage _storage = null;
        public static MySpanStorage GetMySpanStorage()
        {
            var jsonFileHelper = CreateJsonFileHelper();
            return _storage ?? (_storage = new MySpanStorage(jsonFileHelper, () => AppDomain.CurrentDomain.Combine("traces")));
        }

        private static MySpanLoader _spanLoader = null;
        public static MySpanLoader GetMySpanLoader()
        {
            var storage = GetMySpanStorage();
            return _spanLoader ?? (_spanLoader = new MySpanLoader(storage));
        }

        public static MySpanConvert CreateMySpanConvert()
        {
            var convert = new MySpanConvert();
            convert.SpanAssembly = Assembly.LoadFile(AppDomain.CurrentDomain.Combine("Jaeger.dll"));
            return convert;
        }


        private static ILoggerFactory _logFactory = null;
        public static ILoggerFactory GetLoggerFactory()
        {
            return _logFactory ?? (_logFactory = new MyLoggerFactory());
        }
        
        private static MockTracerFactory _mockTracerFactory = null;
        public static MockTracerFactory GetMockTracerFactory(string endPoint)
        {
            return _mockTracerFactory ?? (_mockTracerFactory = new MockTracerFactory(endPoint));
        }
    }
}
