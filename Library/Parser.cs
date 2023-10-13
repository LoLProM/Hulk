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
        throw new Exception($"Not find {type}");
    }

    public ASTree Parse()
    {
        Scope scope = new Scope();
        var expression = ParseAny(scope);
        var endOfFileToken = MatchToken(TokenType.EndOffLineToken);
        return new ASTree(expression, endOfFileToken);
    }
    private HulkExpression ParseAny(Scope scope)
    {
        if (CurrentToken.Type is TokenType.FunctionKeyword)
        {
            return ParseFunctionDeclaration();
        }
        return ParseExpression(scope);
    }

    private FunctionDeclarationExpression ParseFunctionDeclaration()
    {
        var functionKeyword = MatchToken(TokenType.FunctionKeyword);
        var functionName = MatchToken(TokenType.Identifier);
        var functionParameters = ParseParameters();
        var arrowToken = MatchToken(TokenType.ArrowToken);
        var functionScope = new Scope(functionParameters);
        
        var functionBody = ParseFunctionBody(functionScope);


        var functionDeclaration = new FunctionDeclarationExpression(functionName.Text, functionParameters, functionBody);
        if (!Functions.ContainsKey(functionName.Text))
        {
            Functions.Add(functionName.Text, functionDeclaration);
        }
        else
        {
            throw new Exception($"Function {functionName.Text} is already defined");
        }
        return functionDeclaration;
    }

    private string GetFunctionBody()
    {
        var result = "";
        for (int i = CurrentToken.Position; i < tokens.Count; i++)
        {
            if (CurrentToken.Type == TokenType.ColonToken)
            {
                break;
            }
            result += tokens[i].Text;
        }
        return result;
    }

    private HulkExpression ParseFunctionBody(Scope scope)
    {
        return ParseExpression(scope);
    }
    private List<string> ParseParameters()
    {
        TokenAhead();
        var parameters = new List<string>();
        parameters.Add(CurrentToken.Text);
        TokenAhead();
        while (CurrentToken.Type == TokenType.ColonToken)
        {
            TokenAhead();
            parameters.Add(CurrentToken.Text);
            TokenAhead();
        }
        TokenAhead();
        return parameters;
    }
    private HulkExpression ParseLetInExpression(Scope scope)
    {
        var letKeyword = MatchToken(TokenType.LetToken);
        var letExpression = ParseLetExpression(scope);
        var inKeyword = MatchToken(TokenType.InToken);
        var inExpression = ParseExpression(scope.BottomScope());
        return new Let_In_Expression(letExpression, inExpression);
    }
    private LetExpression ParseLetExpression(Scope scope)
    {
        var identifier = MatchToken(TokenType.Identifier);
        var equal = MatchToken(TokenType.SingleEqualToken);
        var expression = ParseExpression(scope.BuildChildScope());
        scope.AddVariable(identifier.Text, expression);
        scope = scope.BuildChildScope();

        if (CurrentToken.Type == TokenType.ColonToken)
        {
            TokenAhead();
            var letChildExpression = ParseLetExpression(scope);
            return new LetExpression(identifier, expression, letChildExpression);
        }
        else
        {
            return new LetExpression(identifier, expression);
        }
    }
    private HulkExpression ParseIfElseExpression(Scope scope)
    {
        var ifKeyword = MatchToken(TokenType.IfKeyword);
        var condition = ParseExpression(scope);
        var ifStatement = ParseExpression(scope);
        MatchToken(TokenType.ElseKeyword);
        var elseStatement = ParseExpression(scope);
        return new If_ElseStatement(ifKeyword, condition, ifStatement, elseStatement);
    }
    private HulkExpression ParseExpression(Scope scope, int actualPrecedence = 0)
    {
        HulkExpression left;
        var unaryOperatorPrecedence = TokensPrecedences.GetUnaryOperatorPrecedence(CurrentToken.Type);
        if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= actualPrecedence)
        {
            var operatorToken = TokenAhead();
            var expression = ParseExpression(scope, unaryOperatorPrecedence);
            left = new HulkUnaryExpression(operatorToken, expression);
        }
        else
        {
            left = ParsePrimary(scope);
        }

        while (true)
        {
            var precedence = TokensPrecedences.GetBinaryOperatorPrecedence(CurrentToken.Type);
            if (precedence == 0 || precedence <= actualPrecedence)
            {
                break;
            }

            var operatorToken = TokenAhead();
            var right = ParseExpression(scope, precedence);
            left = new HulkBinaryExpression(left, operatorToken, right);
        }
        return left;
    }
    private HulkExpression ParsePrimary(Scope scope)
    {
        switch (CurrentToken.Type)
        {
            case TokenType.LetToken:
                return ParseLetInExpression(scope);
            case TokenType.IfKeyword:
                return ParseIfElseExpression(scope);
            case TokenType.StringToken:
                return ParseString();
            case TokenType.OpenParenthesisToken:
                return ParseParenthesizedExpression(scope);
            case TokenType.TrueKeyword:
            case TokenType.FalseKeyword:
                return ParseBoolean();
            case TokenType.NumberToken:
                return ParseNumber();
            case TokenType.FunctionNameToken:
                return ParseFunctionCall(CurrentToken.Text,scope);
            case TokenType.Identifier:
                return ParseIdentifier(CurrentToken, scope);
            default:
                throw new Exception("Invalid Expression");
        }
    }
    private HulkExpression ParseFunctionCall(string identifier, Scope scope)
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
            var expression = ParseExpression(scope);
            parameters.Add(expression);
            if (CurrentToken.Type == TokenType.ColonToken)
            {
                TokenAhead();
            }
        }

        MatchToken(TokenType.CloseParenthesisToken);

        if (!Functions.ContainsKey(identifier))
        {
            throw new Exception($"Function {identifier} is not defined");
        }
        if (Functions[identifier].Arguments.Count != parameters.Count)
        {
            throw new Exception($"Function {identifier} does not have {parameters.Count} parameters but {Functions[identifier].Arguments.Count} parameters");
        }

        var functionDeclaration = Functions[identifier];
        var functionScope = GetFunctionScope(parameters, functionDeclaration.Arguments);
        
        return functionDeclaration.FunctionBody.UseScope(functionScope);
    }
    public Scope GetFunctionScope(List<HulkExpression> functionParameters, List<string> functionDeclarationArguments)
    {
        var scope = new Scope();
        for (int i = 0; i < functionParameters.Count; i++)
        {
            scope.AddVariable(functionDeclarationArguments[i], functionParameters[i]);
        }
        return scope;
    }
    private HulkExpression ParseIdentifier(Token identifier, Scope scope)
    {
        TokenAhead();
        if (scope.Contains(identifier.Text))
        {
            return scope.GetExpression(identifier);
        }
        throw new Exception($"Undefine Variable {identifier}");
    }
    private HulkExpression ParseString()
    {
        var stringToken = MatchToken(TokenType.StringToken);
        return new HulkLiteralExpression(stringToken);
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
    private HulkExpression ParseParenthesizedExpression(Scope scope)
    {
        var left = TokenAhead();
        var expression = ParseExpression(scope);
        var right = MatchToken(TokenType.CloseParenthesisToken);
        return new HulkParenthesesExpression(left, expression, right);
    }
}