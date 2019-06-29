namespace EzTrace.Traces
{
    public interface ITracerFactory
    {
        ITracer GetTracer();
    }
}
