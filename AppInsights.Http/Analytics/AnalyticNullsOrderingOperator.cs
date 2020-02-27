public class AnalyticNullsOrderingOperator
{
    public static AnalyticNullsOrderingOperator NullsFirst = new AnalyticNullsOrderingOperator("nulls first");
    public static AnalyticNullsOrderingOperator NullsLast = new AnalyticNullsOrderingOperator("nulls last");

    private readonly string _nullsOrderingOperator;

    private AnalyticNullsOrderingOperator(string nullsOrderingOperator) => _nullsOrderingOperator = nullsOrderingOperator;

    public override string ToString() => _nullsOrderingOperator;
}