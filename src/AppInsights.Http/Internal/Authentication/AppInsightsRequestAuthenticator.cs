namespace AppInsights.Http.Internal.Authentication
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using AppInsights.Http.Authentication;
    using AppInsights.Http.Configuration;
    using Microsoft.Identity.Client;

    internal class AppInsightsRequestAuthenticator
    {
        private readonly IAppInsightsCredentialProvider _appInsightsCredentialProvider;

        internal AppInsightsRequestAuthenticator() { }

        public AppInsightsRequestAuthenticator(IAppInsightsCredentialProvider appInsightsCredentialProvider)
        {
            _appInsightsCredentialProvider = appInsightsCredentialProvider;
        }

        public virtual async Task AddAuthenticationAsync(HttpRequestMessage request, AppInsightsConfiguration configuration)
        {
            if (!string.IsNullOrWhiteSpace(configuration.ClientId))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await GetTokenAsync(configuration).ConfigureAwait(false));
            }
            else
            {
                request.Headers.TryAddWithoutValidation("x-api-key", configuration.APIKey);
            }
        }

        private async Task<string> GetTokenAsync(AppInsightsConfiguration configuration)
        {
            return (await ConfidentialClientApplicationBuilder
                .CreateWithApplicationOptions(new ConfidentialClientApplicationOptions
                {
                    ClientId = configuration.ClientId,
                    ClientSecret = await _appInsightsCredentialProvider.GetClientSecretAsync().ConfigureAwait(false),
                    TenantId = configuration.TenantId
                })
                .WithAuthority($"https://login.microsoftonline.com/{configuration.TenantId}")
                .Build()
                .AcquireTokenForClient(new [] { "https://api.applicationinsights.io/.default" })
                .ExecuteAsync()
                .ConfigureAwait(false)).AccessToken;
        }
    }
}
