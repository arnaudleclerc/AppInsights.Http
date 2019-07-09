namespace AppInsights.Http.Metrics
{
    public interface IMetricAggregation
    {
        long? Min { get; }
        long? Max { get; }
        long? Avg { get; }
        long? StdDev { get; }
        long? Sum { get; }
    }
}
