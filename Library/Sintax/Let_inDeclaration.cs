namespace HulkProject;

public class Let_In_Expression : HulkExpression
//Esta clase representa que es un let in expression
//Esta constituida por un let expression y una expression
{
    public Let_In_Expression(LetExpression letExpression, HulkExpression inExpression)
    {
        LetExpression = letExpression;
        InExpression = inExpression;
        ExpressionType = InExpression.ExpressionType;
    }
    public LetExpression LetExpression { get; }
    public HulkExpression InExpression { get; }

}
