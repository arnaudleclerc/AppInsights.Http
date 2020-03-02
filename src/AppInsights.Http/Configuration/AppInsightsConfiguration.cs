using System;

namespace AppInsights.Http.Configuration
{
    public class AppInsightsConfiguration
    {
        /// <summary>
        /// ApplicationId defined on AppInsights
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        /// API Key generated on AppInsights. The key needs to have the read telemetry permission
        /// </summary>
        public string APIKey { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public AppInsightsConfiguration() { }

        public AppInsightsConfiguration(string applicationId, string apiKey)
        {
            if (string.IsNullOrWhiteSpace(applicationId))
            {
                throw new ArgumentException("A valid ApplicationId must be given");
            }

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentException("A valid APIKey must be given");
            }

            ApplicationId = applicationId;
            APIKey = apiKey;
        }
    }
}