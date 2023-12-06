namespace HulkProject;

public class HulkLiteralExpression : HulkExpression
//Esta clase es la representacion de una expresion literal (booleanos, numeros, strings..)
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
}




