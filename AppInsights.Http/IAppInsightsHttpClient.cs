using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppInsights.Http
{
    public interface IAppInsightsHttpClient
    {
        Task<IMetric> GetMetricAsync(AppInsights.Http.Metrics metrics);
    }
}
