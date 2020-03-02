namespace AppInsights.Http.Analytics
{
    public class AnalyticsSchema
    {
        public static AnalyticsSchema AvailabilityResults = new AnalyticsSchema("availabilityResults");
        public static AnalyticsSchema BrowserTimings = new AnalyticsSchema("browserTimings");
        public static AnalyticsSchema CustomEvents = new AnalyticsSchema("customEvents");
        public static AnalyticsSchema CustomMetrics = new AnalyticsSchema("customMetrics");
        public static AnalyticsSchema Dependencies = new AnalyticsSchema("dependencies");
        public static AnalyticsSchema Exceptions = new AnalyticsSchema("exceptions");
        public static AnalyticsSchema PageViews = new AnalyticsSchema("pageViews");
        public static AnalyticsSchema PerformanceCounters = new AnalyticsSchema("performanceCounters");
        public static AnalyticsSchema Requests = new AnalyticsSchema("requests");
        public static AnalyticsSchema Traces = new AnalyticsSchema("traces");

        private readonly string _schema;
        private AnalyticsSchema(string schema) => _schema = schema;

        public override string ToString()
        {
            return _schema;
        }
    }
}
