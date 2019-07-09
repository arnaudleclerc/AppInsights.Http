using AppInsights.Http;
using AppInsights.Http.Metrics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AppInsights.Samples.Function
{
    public class GetMetrics
    {
        private readonly IAppInsightsHttpClient _appInsightsHttpClient;

        public GetMetrics(IAppInsightsHttpClient appInsightsHttpClient) => _appInsightsHttpClient = appInsightsHttpClient;

        [FunctionName("GetMetrics")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "{metric}/{aggregation}")] HttpRequest req,
            ILogger logger,
            string metric,
            string aggregation)
        {
            try
            {
                var result = await _appInsightsHttpClient.GetMetricAsync(new MetricsDefinition($"{metric}/{aggregation}"));
                return new OkObjectResult(result);
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                throw ex;
            }
        }
    }
}
