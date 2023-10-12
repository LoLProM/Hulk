namespace HulkProject;

public static class TokensPrecedences
{
    public static int GetUnaryOperatorPrecedence(TokenType type)
    {
        switch (type)
        {
            case TokenType.PlusToken:
            case TokenType.MinusToken:
            case TokenType.NotToken:
                return 7;

            default:
                return 0;
        }
    }

    public static int GetBinaryOperatorPrecedence(TokenType type)
    {
        switch (type)
        {
            case TokenType.ExponentialToken:
                return 6;
                
            case TokenType.MultiplyToken:
            case TokenType.DivisionToken:
            case TokenType.ModuleToken:
                return 5;

            case TokenType.PlusToken:
            case TokenType.MinusToken:
                return 4;

            case TokenType.EqualToken:
            case TokenType.LowerOrEqualToken:
            case TokenType.LowerToken:
            case TokenType.BiggerToken:
            case TokenType.BiggerOrEqualToken:
            case TokenType.NotEqualToken:
                return 3;

            case TokenType.SingleAndToken:
                return 2;
            case TokenType.SingleOrToken:
                return 1;

            default:
                return 0;
        }
    }
}




