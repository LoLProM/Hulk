namespace HulkProject;

public class HulkUnaryExpression : HulkExpression
{
    public HulkUnaryExpression(Token operatorToken, HulkExpression internalExpression)
    {
        OperatorToken = operatorToken;
        InternalExpression = internalExpression;
        ExpressionType = GetUnaryExpressionType();
    }

    public Token OperatorToken { get; }
    public TokenType Type => TokenType.UnaryExpression;
    public HulkExpression InternalExpression { get; }
    private Type GetUnaryExpressionType()
    {
        if (OperatorToken.Type == TokenType.PlusToken)
        {
            if (InternalExpression.ExpressionType == typeof(double))
                return typeof(double);
        }

        else if (OperatorToken.Type == TokenType.MinusToken)
        {
            if (InternalExpression.ExpressionType == typeof(double))
                return typeof(double);
        }

        else if (OperatorToken.Type == TokenType.NotToken)
        {
            if (InternalExpression.ExpressionType == typeof(bool))
                return typeof(bool);
        }
        throw new Exception($"!SEMANTIC ERROR : Cannot applied {OperatorToken.Type} to {InternalExpression.ExpressionType}");
    }
}




