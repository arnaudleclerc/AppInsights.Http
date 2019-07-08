using System;
using System.Collections.Generic;

namespace Azure.AppInsights.API
{
    public interface IMetric
    {
        DateTime Start { get; }
        DateTime End { get; }
        IMetricAggregation Aggregation { get; }
    }
}
