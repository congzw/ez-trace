using System;
using Micro.Shared;
using Microsoft.Extensions.Logging;

namespace Micro.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var loggerFactory = new LoggerFactory().AddConsole())
            {
                var helloTo = "John";
                using (var tracer = DemoFactory.Create("micro-client", loggerFactory))
                {
                    new DemoClient(tracer, loggerFactory).SayHello(helloTo);
                }
            }

            Console.Read();
        }
    }
}
