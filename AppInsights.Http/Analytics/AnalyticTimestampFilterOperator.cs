namespace AppInsights.Http.Analytics
{
    public class AnalyticTimestampFilterOperator
    {
        public static AnalyticTimestampFilterOperator Ago = new AnalyticTimestampFilterOperator("ago");
        public static AnalyticTimestampFilterOperator Datetime = new AnalyticTimestampFilterOperator("datetime");
        public static AnalyticTimestampFilterOperator Now = new AnalyticTimestampFilterOperator("now");

        private readonly string _method;
        private AnalyticTimestampFilterOperator(string method) => _method = method;

        public override string ToString()
        {
            return _method;
        }
    }
}