namespace AppInsights.Http.Analytics
{
    public class AnalyticTimestampFilterOperator
    {
        public static AnalyticTimestampFilterOperator Ago = new AnalyticTimestampFilterOperator("ago");

        private readonly string _method;
        private AnalyticTimestampFilterOperator(string method) => _method = method;

        public override string ToString()
        {
            return _method;
        }
    }
}
