namespace HulkProject;

public class HulkEvaluator
{
    private int Count;
    private readonly int stackOverflow = 1000;
    private HulkExpression _node;
    public HulkEvaluator(HulkExpression node)
    {
        this._node = node;
    }
    public object Evaluate()
    {
        Scope scope = new();
        return EvaluateExpression(_node, scope);
    }
    private object EvaluateExpression(HulkExpression node, Scope scope)
    {
        Count++;
        if (Count > stackOverflow)
        {
            throw new Exception("!OVERFLOW ERROR : Hulk Stack overflow");
        }
        switch (node)
        {
            case HulkLiteralExpression literal:
                return EvaluateLiteralExpression(literal, scope);
            case HulkUnaryExpression unary:
                return EvaluateUnaryExpression(unary, scope);
            case HulkBinaryExpression binaryExpression:
                return EvaluateBinaryExpression(binaryExpression, scope);
            case HulkParenthesesExpression parenthesesExpression:
                return EvaluateParenthesesExpression(parenthesesExpression, scope);
            case If_ElseStatement ifElseStatement:
                return EvaluateIfElseStatement(ifElseStatement, scope);
            case Let_In_Expression letInExpression:
                return EvaluateLetInExpression(letInExpression, scope);
            case PrintDeclaration printDeclaration:
                return EvaluatePrintDeclaration(printDeclaration, scope);
            case FunctionCallExpression functionCallExpression:
                return EvaluateFunctionCallExpression(functionCallExpression, scope);
        }
        throw new Exception($"! SYNTAX ERROR : Unexpected node {node}");
    }

    private object EvaluatePrintDeclaration(PrintDeclaration printDeclaration, Scope scope)
    {
        return EvaluateExpression(printDeclaration.ToPrintExpression, scope);
    }

    private object EvaluateFunctionCallExpression(FunctionCallExpression functionCallExpression, Scope scope)
    {
        if (functionCallExpression.FunctionName is "sin")
        {
            var sinResult = Convert.ToDouble(EvaluateExpression(functionCallExpression.Parameters[0], scope));
            return Math.Sin(sinResult);
        }
        else if (functionCallExpression.FunctionName is "cos")
        {
            var cosResult = Convert.ToDouble(EvaluateExpression(functionCallExpression.Parameters[0], scope));
            return Math.Cos(cosResult);
        }
        else if (functionCallExpression.FunctionName is "log")
        {
            if (functionCallExpression.Parameters.Count == 2)
            {
                var number = Convert.ToDouble(EvaluateExpression(functionCallExpression.Parameters[0], scope));
                var newBase = Convert.ToDouble(EvaluateExpression(functionCallExpression.Parameters[1], scope));
                return Math.Log(number, newBase);
            }
            return Math.Log(Convert.ToDouble(EvaluateExpression(functionCallExpression.Parameters[0], scope)));
        }
        else if (functionCallExpression.FunctionName is "sqrt")
        {
            var squareRootNumber = Convert.ToDouble(EvaluateExpression(functionCallExpression.Parameters[0], scope));
            return Math.Sqrt(squareRootNumber);
        }

        if (!Parser.Functions.ContainsKey(functionCallExpression.FunctionName))
        {
            throw new Exception($"!FUNCTION ERROR : Function {functionCallExpression.FunctionName} is not defined");
        }
        var functionDeclaration = Parser.Functions[functionCallExpression.FunctionName];
        if (functionDeclaration.Arguments.Count != functionCallExpression.Parameters.Count)
        {
            throw new Exception($"!FUNCTION ERROR : Function {functionCallExpression.FunctionName} does not have {functionCallExpression.Parameters.Count} parameters but {Parser.Functions[functionCallExpression.FunctionName].Arguments.Count} parameters");
        }

        var parameters = functionCallExpression.Parameters;
        var arguments = functionDeclaration.Arguments;

        var functionCallScope = new Scope();
        foreach (var (arg, param) in arguments.Zip(parameters))
        {
            functionCallScope.AddVariable(arg, EvaluateExpression(param, scope.BuildChildScope()));
        }

        return EvaluateExpression(functionDeclaration.FunctionBody, functionCallScope);
    }

    private object EvaluateLetInExpression(Let_In_Expression letInExpression, Scope scope)
    {
        var inScope = EvaluateLetExpression(letInExpression.LetExpression, scope);
        var inExpression = EvaluateExpression(letInExpression.InExpression, inScope);
        return inExpression;
    }

    private Scope EvaluateLetExpression(LetExpression letExpression, Scope scope)
    {
        var evaluateLetExpression = EvaluateExpression(letExpression.Expression, scope);
        scope.AddVariable(letExpression.Identifier.Text, evaluateLetExpression);
        if (letExpression.LetChildExpression is null)
        {
            return scope;
        }
        return EvaluateLetExpression(letExpression.LetChildExpression, scope.BuildChildScope());
    }

    private object EvaluateLiteralExpression(HulkLiteralExpression literal, Scope scope)
    {
        if (literal.LiteralToken.Type is TokenType.Identifier)
        {
            return scope.GetValue(literal.LiteralToken);
        }
        return literal.Value;
    }

    private object EvaluateParenthesesExpression(HulkParenthesesExpression parenthesesExpression, Scope scope)
    {
        return EvaluateExpression(parenthesesExpression.InsideExpression, scope.BuildChildScope());
    }

    private object EvaluateUnaryExpression(HulkUnaryExpression unary, Scope scope)
    {
        var value = EvaluateExpression(unary.InternalExpression, scope);
        if (unary.OperatorToken.Type == TokenType.MinusToken)
        {
            return -(double)(value);
        }
        else if (unary.OperatorToken.Type == TokenType.PlusToken)
        {
            return value;
        }
        else if (unary.OperatorToken.Type == TokenType.NotToken)
        {
            return !(bool)value;
        }
        throw new Exception($"!SEMANTIC ERROR : Invalid unary operator {unary.OperatorToken}");
    }
    private object EvaluateBinaryExpression(HulkBinaryExpression binaryExpression, Scope scope)
    {
        var left = EvaluateExpression(binaryExpression.Left, scope);
        var right = EvaluateExpression(binaryExpression.Right, scope);

        switch (binaryExpression.OperatorToken.Type)
        {
            case TokenType.PlusToken:
                if (left.GetType() != right.GetType())
                {
                    throw new Exception($"!SEMANTIC ERROR : Invalid expression: Can't operate {left.GetType()} with {right.GetType()} using {binaryExpression.OperatorToken.Text}");
                }
                return Convert.ToDouble(left) + Convert.ToDouble(right);
            case TokenType.MinusToken:
                if (left.GetType() != right.GetType())
                {
                    throw new Exception($"!SEMANTIC ERROR : Invalid expression: Can't operate {left.GetType()} with {right.GetType()} using {binaryExpression.OperatorToken.Text}");
                }
                return Convert.ToDouble(left) - Convert.ToDouble(right);
            case TokenType.MultiplyToken:
                if (left.GetType() != right.GetType())
                {
                    throw new Exception($"!SEMANTIC ERROR : Invalid expression: Can't operate {left.GetType()} with {right.GetType()} using {binaryExpression.OperatorToken.Text}");
                }
                return Convert.ToDouble(left) * Convert.ToDouble(right);
            case TokenType.DivisionToken:
                if (Convert.ToDouble(right) == 0)
                    throw new Exception("Cannot divide by zero");
                if (left.GetType() != right.GetType())
                {
                    throw new Exception($"!SEMANTIC ERROR : Invalid expression: Can't operate {left.GetType()} with {right.GetType()} using {binaryExpression.OperatorToken.Text}");
                }
                return Convert.ToDouble(left) / Convert.ToDouble(right);
            case TokenType.ModuleToken:
                if (left.GetType() != right.GetType())
                {
                    throw new Exception($"!SEMANTIC ERROR : Invalid expression: Can't operate {left.GetType()} with {right.GetType()} using {binaryExpression.OperatorToken.Text}");
                }
                return Convert.ToDouble(left) % Convert.ToDouble(right);
            case TokenType.ArrobaToken:
                if (left.GetType() != right.GetType())
                {
                    throw new Exception($"!SEMANTIC ERROR : Invalid expression: Can't operate {left.GetType()} with {right.GetType()} using {binaryExpression.OperatorToken.Text}");
                }
                return (string)left + (string)right;
            case TokenType.SingleAndToken:
                if (left.GetType() != right.GetType())
                {
                    throw new Exception($"!SEMANTIC ERROR : Invalid expression: Can't operate {left.GetType()} with {right.GetType()} using {binaryExpression.OperatorToken.Text}");
                }
                return (bool)left && (bool)right;
            case TokenType.SingleOrToken:
                if (left.GetType() != right.GetType())
                {
                    throw new Exception($"!SEMANTIC ERROR : Invalid expression: Can't operate {left.GetType()} with {right.GetType()} using {binaryExpression.OperatorToken.Text}");
                }
                return (bool)left || (bool)right;
            case TokenType.BiggerToken:
                if (left.GetType() != right.GetType())
                {
                    throw new Exception($"!SEMANTIC ERROR : Invalid expression: Can't operate {left.GetType()} with {right.GetType()} using {binaryExpression.OperatorToken.Text}");
                }
                return Convert.ToDouble(left) > Convert.ToDouble(right);
            case TokenType.BiggerOrEqualToken:
                if (left.GetType() != right.GetType())
                {
                    throw new Exception($"!SEMANTIC ERROR : Invalid expression: Can't operate {left.GetType()} with {right.GetType()} using {binaryExpression.OperatorToken.Text}");
                }
                return Convert.ToDouble(left) >= Convert.ToDouble(right);
            case TokenType.LowerToken:
                if (left.GetType() != right.GetType())
                {
                    throw new Exception($"!SEMANTIC ERROR : Invalid expression: Can't operate {left.GetType()} with {right.GetType()} using {binaryExpression.OperatorToken.Text}");
                }
                return Convert.ToDouble(left) < Convert.ToDouble(right);
            case TokenType.LowerOrEqualToken:
                if (left.GetType() != right.GetType())
                {
                    throw new Exception($"!SEMANTIC ERROR : Invalid expression: Can't operate {left.GetType()} with {right.GetType()} using {binaryExpression.OperatorToken.Text}");
                }
                return Convert.ToDouble(left) <= Convert.ToDouble(right);
            case TokenType.EqualToken:
                return Equals(left, right);
            case TokenType.NotEqualToken:
                return !Equals(left, right);
            case TokenType.ExponentialToken:
                if (left.GetType() != right.GetType())
                {
                    throw new Exception($"!SEMANTIC ERROR : Invalid expression: Can't operate {left.GetType()} with {right.GetType()} using {binaryExpression.OperatorToken.Text}");
                }
                return Pow(Convert.ToDouble(left), Convert.ToDouble(right));
            default:
                throw new Exception($"! SEMANTIC ERROR : Unexpected binary operator {binaryExpression.OperatorToken.Type}");
        }
    }
    private object EvaluateIfElseStatement(If_ElseStatement ifElseStatement, Scope scope)
    {
        var condition = (bool)EvaluateExpression(ifElseStatement.IfCondition, scope.BuildChildScope());
        if (condition)
        {
            return EvaluateExpression(ifElseStatement.IfStatement, scope.BuildChildScope());
        }
        else
        {
            return EvaluateExpression(ifElseStatement.ElseClause, scope.BuildChildScope());
        }
    }
    private double Pow(double left, double right)
    {
        if (left == 0 && right == 0) throw new Exception($"!SEMANTIC ERROR : {left} pow to {right} is not defined");
        return Math.Pow(left, right);
    }
}