namespace HulkProject;

public class HulkParenthesesExpression : HulkExpression
{
    public HulkParenthesesExpression(Token openParenthesisToken, HulkExpression insideExpression, Token closeParenthesisToken)
    {
        OpenParenthesisToken = openParenthesisToken;
        InsideExpression = insideExpression;
        CloseParenthesisToken = closeParenthesisToken;
        ExpressionType = insideExpression.ExpressionType;
    }
    public TokenType Type => TokenType.ParenthesizedExpression;
    public Token OpenParenthesisToken { get; }
    public HulkExpression InsideExpression { get; }
    public Token CloseParenthesisToken { get; }

    public override HulkExpression UseScope(Scope functionScope)
    {
        return new HulkParenthesesExpression(
        OpenParenthesisToken,
        InsideExpression.UseScope(functionScope),
        CloseParenthesisToken
        );
    }
}



