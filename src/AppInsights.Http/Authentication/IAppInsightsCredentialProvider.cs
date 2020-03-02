namespace AppInsights.Http.Authentication
{
    using System.Threading.Tasks;

    public interface IAppInsightsCredentialProvider
    {
        /// <summary>
        /// Get the client secret to send to the AppInsights API
        /// </summary>
        /// <returns>ClientSecret to authentication against the AppInsights API</returns>
        Task<string> GetClientSecretAsync();
    }
}
