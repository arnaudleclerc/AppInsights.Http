using System;
using System.Collections.Generic;

namespace AppInsights.Http
{
    public interface IMetric
    {
        DateTime Start { get; }
        DateTime End { get; }
        IMetricAggregation Aggregation { get; }
    }
}
