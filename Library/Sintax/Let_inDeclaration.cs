namespace HulkProject;

public class Let_In_Expression : HulkExpression
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
