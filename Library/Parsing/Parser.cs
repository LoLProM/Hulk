namespace HulkProject;

class Parser
//Segundo paso para el interpreter devolver el arbol de sintaxis abstracta
{
    private int position;
    private List<Token> tokens;
    public Parser(string text)
    {
        var lexer = new Lexer(text);
        tokens = lexer.Tokens;
    }
    private Token CurrentToken => LookAhead(0);
    private Token NextToken => LookAhead(1);
    private Token LookAhead(int offset)//Metodo auxiliar para revisar segun el offset el token correspondiente
    {
        var index = position + offset;
        if (index >= tokens.Count)
        {
            return tokens[index - 1];
        }
        return tokens[index];
    }
    private Token TokenAhead()
    //Consumimos token
    {
        var currentToken = CurrentToken;
        position++;
        return currentToken;
    }
    private Token MatchToken(TokenType type)
    //Verificamos el token que estamos esperando si es el correcto lo consumimos sino lanzamos un error
    {
        if (CurrentToken.Type == type)
        {
            return TokenAhead();
        }
        throw new Exception($"!SYNTAX ERROR : Not find {type} after {LookAhead(-1).Type}");
    }

    public ASTree Parse()//Metodo encargado de comenzar el parseo
    {
        Scope scope = new Scope();
        var expression = ParseAny();
        var endOfFileToken = MatchToken(TokenType.EndOffLineToken);
        return new ASTree(expression, endOfFileToken);
    }
    private HulkExpression ParseAny()//Como en Hulk no podemos hacer una llamada de una funcion dentro de ninguna expression analizamos primero si es una funcion el input del usuario sino parseamos cualquier otra expresion
    {
        if (CurrentToken.Type is TokenType.FunctionKeyword)
        {
            return ParseFunctionDeclaration();
        }
        return ParseExpression();
    }
    private FunctionDeclarationExpression ParseFunctionDeclaration()
    //Parseamos una declaracion de funcion parseando sus parametros su cuerpo y guardando su nombre si ya esta definida lanzamos error ya que en hulk no se puede redefinir
    {
        var functionKeyword = MatchToken(TokenType.FunctionKeyword);
        var functionName = MatchToken(TokenType.Identifier);
        var functionParameters = ParseParameters();
        var arrowToken = MatchToken(TokenType.ArrowToken);
        var functionBody = ParseExpression();
        var functionDeclaration = new FunctionDeclarationExpression(functionName.Text, functionParameters, functionBody);

        if (!StandardLibrary.Functions.ContainsKey(functionName.Text))
        {
            StandardLibrary.Functions.Add(functionName.Text, functionDeclaration);
        }
        else
        {
            throw new Exception($"! FUNCTION ERROR : Function {functionName.Text} is already defined");
        }

        return functionDeclaration;
    }

    private List<string> ParseParameters()
    //Parseamos los parametros 
    {
        MatchToken(TokenType.OpenParenthesisToken);
        var parameters = new List<string>();
        if (CurrentToken.Type is TokenType.CloseParenthesisToken)
        {
            TokenAhead();
            return parameters;
        }
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
    private HulkExpression ParseFunctionCall(string identifier)
    //Parseamos una llamada de funcion analizando cada uno de sus argumentos y parseandolos y los vamos almacenando en una lista de hulk expresion
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
    private HulkExpression ParseIdentifierOrFunctionCall()
    //Analizamos si es un identificador solo o una llamada de funcion
    {
        if (CurrentToken.Type == TokenType.Identifier
        && LookAhead(1).Type == TokenType.OpenParenthesisToken)
        {
            return ParseFunctionCall(CurrentToken.Text);
        }
        return ParseIdentifier(CurrentToken);
    }
    private HulkExpression ParseIdentifier(Token identifier)
    {
        TokenAhead();
        return new HulkLiteralExpression(identifier);
    }
    private HulkExpression ParseLetInExpression()
    //Parseamos el let expression y luego el in expression siempre revisando que machee cada token let e in
    {
        var letKeyword = MatchToken(TokenType.LetToken);
        var letExpression = ParseLetExpression();
        var inKeyword = MatchToken(TokenType.InToken);
        var inExpression = ParseExpression();
        return new Let_In_Expression(letExpression, inExpression);
    }
    private LetExpression ParseLetExpression()
    {
        //Parsear un let seria buscar el identificador y evaluar la expresion a la que esta igualado luego si viene una coma repetimos el proceso ya que seria un hijo del let sino devolvemos la expresion let
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
        //Parseamos la condicion luego el statement luego verificamos si viene la palabra else ya que en hulk es de estricto cumplimiento que un if else expresion contenga la keyword else y luego evaluamos el else
        var ifKeyword = MatchToken(TokenType.IfKeyword);
        var condition = ParseExpression();
        var ifStatement = ParseExpression();
        MatchToken(TokenType.ElseKeyword);
        var elseStatement = ParseExpression();
        return new If_ElseStatement(ifKeyword, condition, ifStatement, elseStatement);
    }
    private HulkExpression ParseExpression(int actualPrecedence = 0)
    {
        //Aqui analizamos expresiones binarias, primero chequeamos la parte izquierda que puede ser cualquier tipo de expression de ahi la recursividad del parser buscamos el operador y analizamos de igual manera la derecha y revisando siempre la precedencia de cada operador para crear el arbol de sintaxis abstracta correcto
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
    private HulkExpression ParsePrimary()//Este metodo analiza el tipo de parseo que se va a efectuar segun el token actual
    {
        switch (CurrentToken.Type)
        {
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
                return ParseIdentifierOrFunctionCall();
            default:
                throw new Exception("! SYNTAX ERROR : Invalid Expression");
        }
    }
    private HulkExpression ParseParenthesizedExpression()
    //Parseamos los parentesis
    {
        var left = TokenAhead();
        var expression = ParseExpression();
        var right = MatchToken(TokenType.CloseParenthesisToken);
        return new HulkParenthesesExpression(left, expression, right);
    }
    private HulkExpression ParseString()
    {
        //Parseamos un string
        var stringToken = MatchToken(TokenType.StringToken);
        var resultStringToken = stringToken.Text[1..^1];//Obtener el string sin comillas
        return new HulkLiteralExpression(stringToken, resultStringToken);
    }
    private HulkExpression ParseNumber()
    {
        //Parseamos un numero
        var number = MatchToken(TokenType.NumberToken);
        var doubleNumber = Convert.ToDouble(number.Value);
        return new HulkLiteralExpression(number, doubleNumber);
    }
    private HulkExpression ParseBoolean()
    {
        //Parseamos un booleano
        var keyword = TokenAhead();
        var booleanExpression = keyword.Type == TokenType.TrueKeyword;
        return new HulkLiteralExpression(keyword, booleanExpression);
    }
}