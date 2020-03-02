namespace AppInsights.Http.Configuration
{
    using System;

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
        /// ID of the application consuming the ApplicationInsights API registered on AAD
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// TenantId of your AAD
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public AppInsightsConfiguration() { }

        /// <summary>
        /// Creates a configuration to call the ApplicationInsights API using ApplicationId / API Key authentication
        /// </summary>
        /// <param name="applicationId">ApplicationId of your application insights instance</param>
        /// <param name="apiKey">API Key to access application insights</param>
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

        /// <summary>
        /// Creates a configuration to call the ApplicationInsights API using OAuth flow
        /// </summary>
        /// <param name="applicationId">ApplicationId of your application insights instance</param>
        /// <param name="clientId">ID of the application consuming the ApplicationInsights API registered on AAD</param>
        /// <param name="tenantId">TenantId of your AAD</param>
        public AppInsightsConfiguration(string applicationId, string clientId, string tenantId)
        {
            if (string.IsNullOrWhiteSpace(applicationId))
            {
                throw new ArgumentException("A valid ApplicationId must be given");
            }

            if (string.IsNullOrWhiteSpace(clientId))
            {
                throw new ArgumentException("A valid ClientId must be given");
            }

            if (string.IsNullOrWhiteSpace(tenantId))
            {
                throw new ArgumentException("A valid TenantId must be given");
            }

            ApplicationId = applicationId;
            ClientId = clientId;
            TenantId = tenantId;
        }
    }
}
