namespace HulkProject;

public class HulkLiteralExpression : HulkExpression
{
    public HulkLiteralExpression(Token literalToken) : this (literalToken,literalToken.Value)
    {
    }

    public HulkLiteralExpression(Token literalToken, object value)
    {
        LiteralToken = literalToken;
        Value = value;
    }


    public override Type ExpressionType => Value.GetType();
    public TokenType Type => TokenType.LiteralExpression;
    public Token LiteralToken { get; }
    public object Value { get; }

    public override HulkExpression UseScope(Scope functionScope)
    {
        if (LiteralToken.Type == TokenType.Identifier) 
        {
            if (functionScope.Contains(LiteralToken.Text))
                return functionScope.GetExpression(LiteralToken);
        }
        return this;
    }
}




