using System.Collections.Generic;
using System.Net;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Propagation;
using OpenTracing.Tag;

namespace Micro.Client
{
    public class DemoClient
    {
        private readonly ITracer _tracer;
        private readonly ILogger<DemoClient> _logger;
        private readonly WebClient _webClient = new WebClient();

        public DemoClient(ITracer tracer, ILoggerFactory loggerFactory)
        {
            _tracer = tracer;
            _logger = loggerFactory.CreateLogger<DemoClient>();
        }
        
        public void SayHello(string helloTo)
        {
            using (var scope = _tracer.BuildSpan("say-hello").StartActive(true))
            {
                scope.Span.SetTag("hello-to", helloTo);
                var helloString = FormatStringFromService(helloTo);
                PrintHello(helloString);
            }
        }

        private string FormatStringFromService(string helloTo)
        {
            using (var scope = _tracer.BuildSpan("format-string").StartActive(true))
            {
                var url = $"http://localhost:5000/api/format/{helloTo}";
                var span = _tracer.ActiveSpan
                    .SetTag(Tags.SpanKind, Tags.SpanKindClient)
                    .SetTag(Tags.HttpMethod, "GET")
                    .SetTag(Tags.HttpUrl, url);

                var dictionary = new Dictionary<string, string>();
                _tracer.Inject(span.Context, BuiltinFormats.HttpHeaders, new TextMapInjectAdapter(dictionary));
                foreach (var entry in dictionary)
                    _webClient.Headers.Add(entry.Key, entry.Value);

                var helloString = _webClient.DownloadString(url);
                scope.Span.Log(new Dictionary<string, object>
                {
                    [LogFields.Event] = "string.Format",
                    ["value"] = helloString
                });
                return helloString;
            }
        }

        private void PrintHello(string helloString)
        {
            using (var scope = _tracer.BuildSpan("print-hello").StartActive(true))
            {
                _logger.LogInformation(helloString);
                scope.Span.Log("WriteLine");
            }
        }
    }
}