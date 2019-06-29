namespace EzTrace.Traces
{
    public interface ITracer
    {
        ISpanBuilder BuildSpan(string opName);
    }
}