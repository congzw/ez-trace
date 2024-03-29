﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;

namespace Micro.Server.Controllers
{
    [Route("api/format")]
    [ApiController]
    public class FormatController : Controller
    {
        private readonly ITracer _tracer;

        public FormatController(ITracer tracer)
        {
            _tracer = tracer;
        }

        // GET: api/format
        [HttpGet]
        public string Get()
        {
            return "Hello!";
        }

        // GET: api/format/helloTo
        [HttpGet("{helloTo}", Name = "GetFormat")]
        public string Get(string helloTo)
        {
            using (var scope = _tracer.BuildSpan("format-controller").StartActive(true))
            {
                var formattedHelloString = $"Hello, {helloTo}!";
                scope.Span.Log(new Dictionary<string, object>
                {
                    [LogFields.Event] = "string-format",
                    ["value"] = formattedHelloString
                });
                return formattedHelloString;
            }
        }
    }
}
