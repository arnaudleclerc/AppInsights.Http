using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Azure.AppInsights.API
{
    public interface IAppInsightsHttpClient
    {
        Task<IMetric> GetMetricAsync(Azure.AppInsights.API.Metrics metrics);
    }
}
