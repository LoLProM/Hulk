namespace HulkProject;

public class HulkParenthesesExpression : HulkExpression
{
//Esta clase representa que es una expresion de parentesis realmente fue creada para mas comodidad a la hora de parsear y evaluar para dar mas prioridad
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
}



