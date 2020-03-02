namespace AppInsights.Http.Exceptions
{
    using System;

    public sealed class AppInsightsException : Exception
    {
        internal AppInsightsException(string message, string code) : base(message)
        {
            Data.Add("AppInsightsException:Code", code);
        }
    }
}
