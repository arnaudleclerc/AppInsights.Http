using AppInsights.Http.Analytics;
using AppInsights.Http.Metadata;
using AppInsights.Http.Metrics;
using System.Threading.Tasks;

namespace AppInsights.Http
{
    public interface IAppInsightsHttpClient
    {
        /// <summary>
        /// Fetch metrics from the application insights API according to the given definition
        /// </summary>
        /// <param name="metrics">Type of metrics to retrieve</param>
        /// <returns>Metrics retrieved from application insights.</returns>
        Task<IMetric> GetMetricAsync(MetricsDefinition metrics);
        /// <summary>
        /// Fetch metrics from the application insights API according to the given definition
        /// </summary>
        /// <param name="metrics">Type of metrics to retrieve</param>
        /// <param name="applicationId">ID of the application</param>
        /// <returns>Metrics retrieved from application insights.</returns>
        Task<IMetric> GetMetricAsync(MetricsDefinition metrics, string applicationId);
        /// <summary>
        /// Fetch the metadata from the application insights API
        /// </summary>
        /// <returns>Metadata retrieved from application insights.</returns>
        Task<IMetadata> GetMetadataAsync();
        /// <summary>
        /// Fetch the metadata from the application insights API
        /// </summary>
        /// <param name="applicationId">ID of the application</param>
        /// <returns>Metadata retrieved from application insights.</returns>
        Task<IMetadata> GetMetadataAsync(string applicationId);
        /// <summary>
        /// Executes an analytics query
        /// </summary>
        /// <param name="analyticsQueryBuilder">Query to execute</param>
        /// <returns>Result of the query</returns>
        Task<IAnalyticQueryResult> QueryAsync(IAnalyticsQueryBuilder analyticsQueryBuilder);
        /// <summary>
        /// Executes an analytics query
        /// </summary>
        /// <param name="analyticsQueryBuilder">Query to execute</param>
        /// <param name="applicationId">ID of the application</param>
        /// <returns>Result of the query</returns>
        Task<IAnalyticQueryResult> QueryAsync(IAnalyticsQueryBuilder analyticsQueryBuilder, string applicationId);
        /// <summary>
        /// Executes an analytics query
        /// </summary>
        /// <param name="query">Query to execute</param>
        /// <returns>Result of the query</returns>
        Task<IAnalyticQueryResult> QueryAsync(string query);
        /// <summary>
        /// Executes an analytics query
        /// </summary>
        /// <param name="query">Query to execute</param>
        /// <param name="applicationId">ID of the application</param>
        /// <returns>Result of the query</returns>
        Task<IAnalyticQueryResult> QueryAsync(string query, string applicationId);
    }
}
