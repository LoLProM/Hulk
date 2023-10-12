namespace HulkProject;

public class Let_In_Expression : HulkExpression
{
    public Let_In_Expression(LetExpression letExpression, HulkExpression inExpression)
    {
        LetExpression = letExpression;
        InExpression = inExpression;
        ExpressionType = inExpression.ExpressionType;
    }
    public LetExpression LetExpression { get; }
    public HulkExpression InExpression { get; }

    public override HulkExpression UseScope(Scope functionScope)
    {
        return new Let_In_Expression(
            (LetExpression)LetExpression.UseScope(functionScope),
            InExpression.UseScope(functionScope)
            );
    }
}
