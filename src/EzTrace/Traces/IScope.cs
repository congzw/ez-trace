using System;

namespace EzTrace.Traces
{
    public interface IScope : IDisposable
    {
        ISpan Span { get; }
    }
}