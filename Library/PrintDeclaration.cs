namespace HulkProject;

public class PrintDeclaration : HulkExpression
{
    public PrintDeclaration(Token printKeyword, HulkExpression toPrintExpression)
    {
        PrintKeyword = printKeyword;
        ToPrintExpression = toPrintExpression;
        ExpressionType = toPrintExpression.ExpressionType;
    }

    public Token PrintKeyword { get; }
    public HulkExpression ToPrintExpression { get; }

    public override HulkExpression UseScope(Scope functionScope)
    {
        return this;
    }
}