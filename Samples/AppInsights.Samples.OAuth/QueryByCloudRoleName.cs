using AppInsights.Http;
using AppInsights.Http.Analytics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AppInsights.Samples.OAuth
{
    public class QueryByCloudRoleName
    {
        private readonly IAnalyticsQueryBuilderFactory _analyticsQueryBuilderFactory;
        private readonly IAppInsightsHttpClient _appInsightsHttpClient;

        public QueryByCloudRoleName(IAnalyticsQueryBuilderFactory analyticsQueryBuilderFactory,
            IAppInsightsHttpClient appInsightsHttpClient)
        {
            _analyticsQueryBuilderFactory = analyticsQueryBuilderFactory;
            _appInsightsHttpClient = appInsightsHttpClient;
        }

        [FunctionName("QueryByCloudRoleName")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "query/requests/cloudrolename/{cloudRoleName}")] HttpRequest req,
            ILogger logger,
            string cloudRoleName)
        {
            try
            {
                var builder = _analyticsQueryBuilderFactory.CreateAnalyticsQueryBuilder(AnalyticsSchema.Requests)
                    .WithFilter("cloud_RoleName", AnalyticFilterOperator.Equal, cloudRoleName)
                    .WithSummarizeCount("name", "resultCode");
                var result = await _appInsightsHttpClient.QueryAsync(builder);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw ex;
            }
        }
    }
}
