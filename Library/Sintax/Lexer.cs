namespace HulkProject;

using System.Text.RegularExpressions;

class Lexer
//Esta clase representa el primer paso del interprete aqui se tokeniza la entrada y se devuelve una lista de tokens
{
    public List<Token> Tokens = new List<Token>();
    int position = 0;
    public Lexer(string textInput)
    {
        //Se utilizo regex para mas comodidad a la hora de recibir el input del usuario
        textInput = Regex.Replace(textInput, @"\s+", " ");

        Regex SyntaxTokens = new(@"\d+[^\D]|^-{0,1}\d+$|^-{0,1}\d+\.\d+E(\+|-)\d+$|^-{0,1}\d+\.\d+$|\+|\-|\*|\^|%|\(|\)|(=>)|(>=)|(<=)|<[=]{0}|>[=]{0}|!=|;|,|={1,2}|!|\&|\||(\u0022([^\u0022\\]|\\.)*\u0022)|@|[a-zA-Z]+\w*|/|[^\(\)\+\-\*/\^%<>=!&\|,;\s]+");

        var matches = SyntaxTokens.Matches(textInput);

        foreach (Match match in SyntaxTokens.Matches(textInput))
        {
            //Por cada matcheo vamos viendo que tipo de token es el actual y lo vamos ubicando en la lista
            var token = GetToken(match);
            Tokens.Add(token);
        }
    }
    private Token GetToken(Match match)
    //Metodo encargado de analizar cada token
    {
        if (IsNumber(match))
        {
            double.TryParse(match.Value, out var value);
            return new Token(match.Value, TokenType.NumberToken, match.Index, value);
        }

        else if (IsString(match))
        {
            return new Token(match.Value, TokenType.StringToken, match.Index, match.Value);
        }
        else if (IsTrueKeyWord(match))
        {
            return new Token(match.Value, TokenType.TrueKeyword, match.Index, match.Value);
        }

        else if (IsFalseKeyWord(match))
        {
            return new Token(match.Value, TokenType.FalseKeyword, match.Index, match.Value);
        }
        else if (IsEndOffLine(match))
        {
            return new Token(match.Value, TokenType.EndOffLineToken, match.Index, match.Value);
        }
        else if (IsLetToken(match))
        {
            return new Token(match.Value, TokenType.LetToken, match.Index, match.Value);
        }
        else if (IsInToken(match))
        {
            return new Token(match.Value, TokenType.InToken, match.Index, match.Value);
        }
        else if (IsIfKeyword(match))
        {
            return new Token(match.Value, TokenType.IfKeyword, match.Index, match.Value);
        }
        else if (IsElseKeyword(match))
        {
            return new Token(match.Value, TokenType.ElseKeyword, match.Index, match.Value);
        }
        else if (IsFunction(match))
        {
            return new Token(match.Value, TokenType.FunctionKeyword, match.Index, match.Value);
        }
        else if (IsIdentifier(match))
        {
            return new Token(match.Value, TokenType.Identifier, match.Index, match.Value);
        }

        switch (match.Value)
        {
            case "+":
                return new Token("+", TokenType.PlusToken, match.Index, null);
            case "-":
                return new Token("-", TokenType.MinusToken, match.Index, null);
            case "*":
                return new Token("*", TokenType.MultiplyToken, match.Index, null);
            case "/":
                return new Token("/", TokenType.DivisionToken, match.Index, null);
            case "(":
                return new Token("(", TokenType.OpenParenthesisToken, match.Index, null);
            case ")":
                return new Token(")", TokenType.CloseParenthesisToken, match.Index, null);
            case "!":
                return new Token("!", TokenType.NotToken, match.Index, null);
            case "<":
                return new Token("<", TokenType.LowerToken, match.Index, null);
            case ">":
                return new Token(">", TokenType.BiggerToken, match.Index, null);
            case "<=":
                return new Token("<=", TokenType.LowerOrEqualToken, match.Index, null);
            case ">=":
                return new Token(">=", TokenType.BiggerOrEqualToken, match.Index, null);
            case "!=":
                return new Token("!=", TokenType.NotEqualToken, match.Index, null);
            case "==":
                return new Token("==", TokenType.EqualToken, match.Index, null);
            case "=":
                return new Token("=", TokenType.SingleEqualToken, match.Index, null);
            case "&":
                return new Token("&", TokenType.SingleAndToken, match.Index, null);
            case "|":
                return new Token("|", TokenType.SingleOrToken, match.Index, null);
            case "^":
                return new Token("^", TokenType.ExponentialToken, match.Index, null);
            case "%":
                return new Token("%", TokenType.ModuleToken, match.Index, null);
            case ",":
                return new Token(",", TokenType.ColonToken, match.Index, null);
            case "=>":
                return new Token("=>", TokenType.ArrowToken, match.Index, null);
            case "@":
                return new Token("@", TokenType.ArrobaToken, match.Index, null);
        }
        throw new Exception($"! LEXICAL ERROR : '{match.Value}' is not a valid token");
    }
    private bool IsFunction(Match match)
    {
        if (match.Value == "function")
        {
            position++;
            return true;
        }
        return false;
    }

    private bool IsElseKeyword(Match match)
    {
        if (match.Value == "else")
        {
            position++;
            return true;
        }
        return false;
    }

    private bool IsIfKeyword(Match match)
    {
        if (match.Value == "if")
        {
            position++;
            return true;
        }
        return false;
    }

    private bool IsIdentifier(Match match)
    {
        foreach (var letter in match.Value)
        {
            if (!char.IsLetter(letter))
            {
                return false;
            }
        }
        return true;
    }

    private bool IsInToken(Match match)
    {
        if (match.Value == "in")
        {
            position++;
            return true;
        }
        return false;
    }

    private bool IsLetToken(Match match)
    {
        if (match.Value == "let")
        {
            position++;
            return true;
        }
        return false;
    }

    private bool IsFalseKeyWord(Match match)
    {
        if (match.Value == "false")
        {
            position++;
            return true;
        }
        return false;
    }

    private bool IsTrueKeyWord(Match match)
    {
        if (match.Value == "true")
        {
            position++;
            return true;
        }
        return false;
    }

    private bool IsEndOffLine(Match match)
    {
        if (match.Value == ";")
        {
            position++;
            return true;
        }
        return false;
    }

    private bool IsString(Match match)
    {
        var matchString = match.Value.ToString();
        var count = 0;

        foreach (var character in matchString)
        {
            if (character == '"')
                count++;
            continue;
        }
        if (count == 2)
        {
            position++;
            return true;
        }
        return false;
    }
    private bool IsNumber(Match match)
    {
        var count = 0;
        var position = 0;
        var separator = 0;

        while (char.IsDigit(match.Value[position]))
        {
            position++;
            count++;
            if (position >= match.Value.Length)
            {
                break;
            }
            if (match.Value[position] == '.')
            {
                separator++;
                count++;
                position++;
                continue;
            }
        }
        if (count != match.Value.Length || separator > 1)
        {
            return false;
        }
        else
        {
            position++;
            return true;
        }
    }
}

