namespace HulkProject;

class Parser
{
    private int position;
    private List<Token> tokens;
    public static Dictionary<string, FunctionDeclarationExpression?> Functions = new();
    public Parser(string text)
    {
        var lexer = new Lexer(text);
        tokens = lexer.Tokens;
    }

    private Token CurrentToken => LookAhead(0);
    private Token NextToken => LookAhead(1);
    private Token LookAhead(int offset)
    {
        var index = position + offset;
        if (index >= tokens.Count)
        {
            return tokens[index - 1];
        }
        return tokens[index];
    }
    private Token TokenAhead()
    {
        var currentToken = CurrentToken;
        position++;
        return currentToken;
    }
    private Token MatchToken(TokenType type)
    {
        if (CurrentToken.Type == type)
        {
            return TokenAhead();
        }
        throw new Exception($"!SYNTAX ERROR : Not find {type} after {LookAhead(-1).Type} {LookAhead(-1).Text}");
    }

    public ASTree Parse()
    {
        Scope scope = new Scope();
        var expression = ParseAny();
        var endOfFileToken = MatchToken(TokenType.EndOffLineToken);
        return new ASTree(expression, endOfFileToken);
    }
    private HulkExpression ParseAny()
    {
        if (CurrentToken.Type is TokenType.FunctionKeyword)
        {
            return ParseFunctionDeclaration();
        }
        return ParseExpression();
    }

    private FunctionDeclarationExpression ParseFunctionDeclaration()
    {
        var functionKeyword = MatchToken(TokenType.FunctionKeyword);
        var functionName = MatchToken(TokenType.Identifier);
        var functionParameters = ParseParameters();
        var arrowToken = MatchToken(TokenType.ArrowToken);
        var functionBody = ParseExpression();
        var functionDeclaration = new FunctionDeclarationExpression(functionName.Text, functionParameters, functionBody);

        if (!Functions.ContainsKey(functionName.Text))
        {
            Functions.Add(functionName.Text, functionDeclaration);
        }
        else
        {
            throw new Exception($"! FUNCTION ERROR : Function {functionName.Text} is already defined");
        }

        return functionDeclaration;
    }

    private List<string> ParseParameters()
    {
        MatchToken(TokenType.OpenParenthesisToken);
        var parameters = new List<string>();
        parameters.Add(CurrentToken.Text);
        TokenAhead();
        while (CurrentToken.Type == TokenType.ColonToken)
        {
            TokenAhead();
            if (CurrentToken.Type is not TokenType.Identifier)
            {
                throw new Exception($"! SEMANTIC ERROR : Parameters must be a valid identifier");
            }
            if (parameters.Contains(CurrentToken.Text))
            {
                throw new Exception($"! SEMANTIC ERROR : A parameter with the name '{CurrentToken.Text}' already exists insert another parameter name");
            }
            parameters.Add(CurrentToken.Text);
            TokenAhead();
        }
        TokenAhead();
        return parameters;
    }
    private HulkExpression ParseLetInExpression()
    {
        var letKeyword = MatchToken(TokenType.LetToken);
        var letExpression = ParseLetExpression();
        var inKeyword = MatchToken(TokenType.InToken);
        var inExpression = ParseExpression();
        return new Let_In_Expression(letExpression, inExpression);
    }
    private LetExpression ParseLetExpression()
    {
        var identifier = MatchToken(TokenType.Identifier);
        var equal = MatchToken(TokenType.SingleEqualToken);
        var expression = ParseExpression();

        if (CurrentToken.Type == TokenType.ColonToken)
        {
            var comma = MatchToken(TokenType.ColonToken);
            var letChildExpression = ParseLetExpression();
            return new LetExpression(identifier, expression, letChildExpression);
        }
        else
        {
            return new LetExpression(identifier, expression);
        }
    }
    private HulkExpression ParseIfElseExpression()
    {
        var ifKeyword = MatchToken(TokenType.IfKeyword);
        var condition = ParseExpression();
        var ifStatement = ParseExpression();
        MatchToken(TokenType.ElseKeyword);
        var elseStatement = ParseExpression();
        return new If_ElseStatement(ifKeyword, condition, ifStatement, elseStatement);
    }
    private HulkExpression ParseExpression(int actualPrecedence = 0)
    {
        HulkExpression left;
        var unaryOperatorPrecedence = TokensPrecedences.GetUnaryOperatorPrecedence(CurrentToken.Type);
        if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= actualPrecedence)
        {
            var operatorToken = TokenAhead();
            var expression = ParseExpression(unaryOperatorPrecedence);
            left = new HulkUnaryExpression(operatorToken, expression);
        }
        else
        {
            left = ParsePrimary();
        }

        while (true)
        {
            var precedence = TokensPrecedences.GetBinaryOperatorPrecedence(CurrentToken.Type);
            if (precedence == 0 || precedence <= actualPrecedence)
            {
                break;
            }

            var operatorToken = TokenAhead();
            var right = ParseExpression(precedence);
            left = new HulkBinaryExpression(left, operatorToken, right);
        }
        return left;
    }
    private HulkExpression ParsePrimary()
    {
        switch (CurrentToken.Type)
        {
            case TokenType.PrintKeyword:
                return ParsePrintKeyword();
            case TokenType.LetToken:
                return ParseLetInExpression();
            case TokenType.IfKeyword:
                return ParseIfElseExpression();
            case TokenType.StringToken:
                return ParseString();
            case TokenType.OpenParenthesisToken:
                return ParseParenthesizedExpression();
            case TokenType.TrueKeyword:
            case TokenType.FalseKeyword:
                return ParseBoolean();
            case TokenType.NumberToken:
                return ParseNumber();

            case TokenType.Identifier:
            case TokenType.SinKeyword:
            case TokenType.CosKeyword:
            case TokenType.RandKeyword :
            case TokenType.SQRTKeyword:
                return ParseIdentifierOrFunctionCall();

            case TokenType.PIKeyword:
                return ParsePIKeyword();
            case TokenType.EulerKeyword:
                return ParseEulerKeyword();
            default:
                throw new Exception("! LEXICAL ERROR : Invalid Expression");
        }
    }

    private HulkExpression ParseEulerKeyword()
    {
        var euler = MatchToken(TokenType.EulerKeyword);
        return new HulkLiteralExpression(euler, Math.E);
    }

    private HulkExpression ParsePIKeyword()
    {
        var PI = MatchToken(TokenType.PIKeyword);
        return new HulkLiteralExpression(PI, Math.PI);
    }

    private HulkExpression ParseIdentifierOrFunctionCall()
    {
        if ((CurrentToken.Type == TokenType.Identifier || CurrentToken.Type == TokenType.SinKeyword ||
        CurrentToken.Type == TokenType.CosKeyword ||
        CurrentToken.Type == TokenType.RandKeyword ||
        CurrentToken.Type == TokenType.SQRTKeyword)
        && LookAhead(1).Type == TokenType.OpenParenthesisToken)
        {
            return ParseFunctionCall(CurrentToken.Text);
        }
        return ParseIdentifier(CurrentToken);
    }
    private HulkExpression ParsePrintKeyword()
    {
        var printKeyword = MatchToken(TokenType.PrintKeyword);
        var expression = ParseExpression();
        return new PrintDeclaration(printKeyword, expression);
    }

    private HulkExpression ParseFunctionCall(string identifier)
    {
        TokenAhead();
        var parameters = new List<HulkExpression>();

        MatchToken(TokenType.OpenParenthesisToken);
        while (true)
        {
            if (CurrentToken.Type == TokenType.CloseParenthesisToken)
            {
                break;
            }
            var expression = ParseExpression();
            parameters.Add(expression);
            if (CurrentToken.Type == TokenType.ColonToken)
            {
                TokenAhead();
            }
        }

        MatchToken(TokenType.CloseParenthesisToken);

        return new FunctionCallExpression(identifier, parameters);
    }

    private HulkExpression ParseIdentifier(Token identifier)
    {
        TokenAhead();
        return new HulkLiteralExpression(identifier);
    }
    private HulkExpression ParseString()
    {
        var stringToken = MatchToken(TokenType.StringToken);
        var resultStringToken = stringToken.Text[1..^1];//Obtener el string sin comillas
        return new HulkLiteralExpression(stringToken, resultStringToken);
    }
    private HulkExpression ParseNumber()
    {
        var number = MatchToken(TokenType.NumberToken);
        return new HulkLiteralExpression(number);
    }
    private HulkExpression ParseBoolean()
    {
        var keyword = TokenAhead();
        var booleanExpression = keyword.Type == TokenType.TrueKeyword;
        return new HulkLiteralExpression(keyword, booleanExpression);
    }
    private HulkExpression ParseParenthesizedExpression()
    {
        var left = TokenAhead();
        var expression = ParseExpression();
        var right = MatchToken(TokenType.CloseParenthesisToken);
        return new HulkParenthesesExpression(left, expression, right);
    }
}