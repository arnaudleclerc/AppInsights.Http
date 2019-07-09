namespace AppInsights.Http
{
    /// <summary>
    /// Metrics which can be used
    /// </summary>
    public class Metrics
    {
        public static Metrics AvailabilityResultsAvailabilityPercentage = new Metrics("availabilityResults/availabilityPercentage");
        public static Metrics AvailabilityResultsCount = new Metrics("availabilityResults/count");
        public static Metrics AvailabilityResultsDuration = new Metrics("availabilityResults/duration");

        public static Metrics BrowserTimingNetworkDuration = new Metrics("browserTimings/networkDuration");
        public static Metrics BrowserTimingsProcessingDuratiokn = new Metrics("browserTimings/processingDuration");
        public static Metrics BrowserTimingsReceiveDuration = new Metrics("browserTimings/receiveDuration");
        public static Metrics BrowserTimingsSendDuration = new Metrics("browserTimings/sendDuration");
        public static Metrics BrowserTimingsTotalDuration = new Metrics("browserTimings/totalDuration");

        public static Metrics CustomEvents = new Metrics("customEvents/count");

        public static Metrics DependenciesCount = new Metrics("dependencies/count");
        public static Metrics DependenciesDuration = new Metrics("dependencies/duration");
        public static Metrics DependenciesFailed = new Metrics("dependencies/failed");

        public static Metrics ExceptionsBrowser = new Metrics("exceptions/browser");
        public static Metrics ExceptionsCount = new Metrics("exceptions/count");
        public static Metrics ExceptionsServer = new Metrics("exceptions/server");

        public static Metrics PageViewDuration = new Metrics("pageViews/duration");

        public static Metrics PerformanceCountersExceptionsPerSecond = new Metrics("performanceCounters/exceptionsPerSecond");
        public static Metrics PerformanceCountersMemoryAvailableBytes = new Metrics("performanceCounters/memoryAvailableBytes");
        public static Metrics PerformanceCountersProcessCpuPercentage = new Metrics("performanceCounters/processCpuPercentage");
        public static Metrics PerformanceCountersProcessIoBytesPerSecond = new Metrics("performanceCounters/processIOBytesPerSecond");
        public static Metrics PerformanceCountersProcessorCpuPercentage = new Metrics("performanceCounters/processorCpuPercentage");
        public static Metrics PerformanceCountersProcessPrivateBytes = new Metrics("performanceCounters/processPrivateBytes");
        public static Metrics PerformanceCountersRequestExecutionTime = new Metrics("performanceCounters/requestExecutionTime");
        public static Metrics PerformanceCountersRequestsPerSecond = new Metrics("performanceCounters/requestsPerSecond");
        public static Metrics PerformanceCountersRequestsRequestsInQueue = new Metrics("performanceCounters/requestsInQueue");

        public static Metrics RequestsCount = new Metrics("requests/count");
        public static Metrics RequestsDuration = new Metrics("requests/duration");
        public static Metrics RequestsFailed = new Metrics("requests/failed");

        public static Metrics SessionsCount = new Metrics("sessions/count");

        public static Metrics UsersAuthenticated = new Metrics("users/authenticated");
        public static Metrics UsersCount = new Metrics("users/count");

        private readonly string _metric;

        private Metrics(string metric) => _metric = metric;

        public override string ToString()
        {
            return _metric;
        }
    }
}
