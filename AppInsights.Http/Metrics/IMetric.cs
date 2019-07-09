using System;

namespace AppInsights.Http.Metrics
{
    public interface IMetric
    {
        DateTime Start { get; }
        DateTime End { get; }
        IMetricAggregation Aggregation { get; }
    }
}
