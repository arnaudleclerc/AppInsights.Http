namespace AppInsights.Http.Metrics
{
    /// <summary>
    /// Metrics which can be used
    /// </summary>
    public class MetricsDefinition
    {
        public static MetricsDefinition AvailabilityResultsAvailabilityPercentage = new MetricsDefinition("availabilityResults/availabilityPercentage");
        public static MetricsDefinition AvailabilityResultsCount = new MetricsDefinition("availabilityResults/count");
        public static MetricsDefinition AvailabilityResultsDuration = new MetricsDefinition("availabilityResults/duration");

        public static MetricsDefinition BrowserTimingNetworkDuration = new MetricsDefinition("browserTimings/networkDuration");
        public static MetricsDefinition BrowserTimingsProcessingDuratiokn = new MetricsDefinition("browserTimings/processingDuration");
        public static MetricsDefinition BrowserTimingsReceiveDuration = new MetricsDefinition("browserTimings/receiveDuration");
        public static MetricsDefinition BrowserTimingsSendDuration = new MetricsDefinition("browserTimings/sendDuration");
        public static MetricsDefinition BrowserTimingsTotalDuration = new MetricsDefinition("browserTimings/totalDuration");

        public static MetricsDefinition CustomEvents = new MetricsDefinition("customEvents/count");

        public static MetricsDefinition DependenciesCount = new MetricsDefinition("dependencies/count");
        public static MetricsDefinition DependenciesDuration = new MetricsDefinition("dependencies/duration");
        public static MetricsDefinition DependenciesFailed = new MetricsDefinition("dependencies/failed");

        public static MetricsDefinition ExceptionsBrowser = new MetricsDefinition("exceptions/browser");
        public static MetricsDefinition ExceptionsCount = new MetricsDefinition("exceptions/count");
        public static MetricsDefinition ExceptionsServer = new MetricsDefinition("exceptions/server");

        public static MetricsDefinition PageViewDuration = new MetricsDefinition("pageViews/duration");

        public static MetricsDefinition PerformanceCountersExceptionsPerSecond = new MetricsDefinition("performanceCounters/exceptionsPerSecond");
        public static MetricsDefinition PerformanceCountersMemoryAvailableBytes = new MetricsDefinition("performanceCounters/memoryAvailableBytes");
        public static MetricsDefinition PerformanceCountersProcessCpuPercentage = new MetricsDefinition("performanceCounters/processCpuPercentage");
        public static MetricsDefinition PerformanceCountersProcessIoBytesPerSecond = new MetricsDefinition("performanceCounters/processIOBytesPerSecond");
        public static MetricsDefinition PerformanceCountersProcessorCpuPercentage = new MetricsDefinition("performanceCounters/processorCpuPercentage");
        public static MetricsDefinition PerformanceCountersProcessPrivateBytes = new MetricsDefinition("performanceCounters/processPrivateBytes");
        public static MetricsDefinition PerformanceCountersRequestExecutionTime = new MetricsDefinition("performanceCounters/requestExecutionTime");
        public static MetricsDefinition PerformanceCountersRequestsPerSecond = new MetricsDefinition("performanceCounters/requestsPerSecond");
        public static MetricsDefinition PerformanceCountersRequestsRequestsInQueue = new MetricsDefinition("performanceCounters/requestsInQueue");

        public static MetricsDefinition RequestsCount = new MetricsDefinition("requests/count");
        public static MetricsDefinition RequestsDuration = new MetricsDefinition("requests/duration");
        public static MetricsDefinition RequestsFailed = new MetricsDefinition("requests/failed");

        public static MetricsDefinition SessionsCount = new MetricsDefinition("sessions/count");

        public static MetricsDefinition UsersAuthenticated = new MetricsDefinition("users/authenticated");
        public static MetricsDefinition UsersCount = new MetricsDefinition("users/count");

        private readonly string _metric;

        public MetricsDefinition(string metric) => _metric = metric;

        public override string ToString()
        {
            return _metric;
        }
    }
}
