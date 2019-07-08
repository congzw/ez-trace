namespace TraceServer.Domain.Traces
{
    public interface IClientSpanLocate
    {
        string TraceId { get; set; }
        string SpanId { get; set; }
        string ParentSpanId { get; set; }
    }
    public static class ClientSpanLocateExtensions
    {
        public static string ToLocateKey(this IClientSpanLocate locate)
        {
            if (locate == null)
            {
                return null;
            }
            return string.IsNullOrWhiteSpace(locate.ParentSpanId) ?
                string.Format("{0}-{1}", locate.TraceId, locate.SpanId) :
                string.Format("{0}-{1}-{2}", locate.TraceId, locate.ParentSpanId, locate.SpanId);
        }

        public static T AutoSet<T>(this T locate, IClientSpanLocate setter) where T : IClientSpanLocate
        {
            if (locate == null || setter == null)
            {
                return locate;
            }

            locate.SpanId = setter.SpanId;
            locate.TraceId = setter.TraceId;
            locate.ParentSpanId = setter.ParentSpanId;
            return locate;
        }

    }
}