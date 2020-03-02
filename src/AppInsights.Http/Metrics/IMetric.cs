namespace AppInsights.Http.Metrics
{
    using System;

    public interface IMetric
    {
        DateTime Start { get; }
        DateTime End { get; }
        IMetricAggregation Aggregation { get; }
    }
}
