using System;
using Microsoft.Extensions.Logging;

namespace EzTrace.Helpers
{
    public class MyLoggerFactory : ILoggerFactory
    {
        public MyLogger DefaultLogger { get; set; }

        public MyLoggerFactory()
        {
            DefaultLogger = new MyLogger(new MyLogHelper("Default"));
        }

        public void Dispose()
        {
            DefaultLogger.Dispose();
        }

        public ILogger CreateLogger(string categoryName)
        {
            return DefaultLogger;
        }

        public void AddProvider(ILoggerProvider provider)
        {
        }
    }

    public class MyLogger : ILogger, IDisposable
    {
        public MyLogger(MyLogHelper myLogHelper)
        {
            LogHelper = myLogHelper;

        }
        public MyLogHelper LogHelper { get; set; }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (exception != null)
            {
                LogHelper.InfoException(exception);
                return;
            }
            LogHelper.Info(formatter(state, null));
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return this;
        }

        public void Dispose()
        {
        }
    }
}
