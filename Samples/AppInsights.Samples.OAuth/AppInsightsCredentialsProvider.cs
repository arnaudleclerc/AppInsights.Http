namespace AppInsights.Samples.OAuth
{
    using System.Threading.Tasks;
    using AppInsights.Http.Authentication;

    public class AppInsightsCredentialProvider : IAppInsightsCredentialProvider
    {
        private readonly string _clientSecret;

        public AppInsightsCredentialProvider(string clientSecret)
        {
            _clientSecret = clientSecret;
        }

        public Task<string> GetClientSecretAsync()
        {
            return Task.FromResult(_clientSecret);
        }
    }
}
