using AppInsights.Http.Metadata;
using AppInsights.Http.Metrics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppInsights.Http
{
    public interface IAppInsightsHttpClient
    {
        Task<IMetric> GetMetricAsync(MetricsDefinition metrics);
        Task<IMetadata> GetMetadataAsync();
    }
}
