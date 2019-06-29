namespace EzTrace.Traces
{
    public interface ISpanBuilder
    {
        IScope StartActive(bool finishSpanOnDispose);

    }
}