using AppInsights.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AppInsights.Samples.Function
{
    public class GetMetadata
    {
        private readonly IAppInsightsHttpClient _appInsightsHttpClient;

        public GetMetadata(IAppInsightsHttpClient appInsightsHttpClient) => _appInsightsHttpClient = appInsightsHttpClient;

        [FunctionName("GetMetadata")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "metadata")] HttpRequest req,
            ILogger logger)
        {
            try
            {
                var result = await _appInsightsHttpClient.GetMetadataAsync();
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
