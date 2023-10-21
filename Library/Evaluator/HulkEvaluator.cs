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
        Scope scope = StandardLibrary.Variables;
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
            case FunctionCallExpression functionCallExpression:
                return EvaluateFunctionCallExpression(functionCallExpression, scope);
            case FunctionReference functionReference:
                return EvaluateFunctionReference(functionReference, scope);
            
        }
        throw new Exception($"! SYNTAX ERROR : Unexpected node {node}");
    }

    private object EvaluateFunctionReference(FunctionReference functionReference, Scope scope)
    {
        return functionReference.Eval(scope);
    }

    private object EvaluateFunctionCallExpression(FunctionCallExpression functionCallExpression, Scope scope)
    {
        if (!StandardLibrary.Functions.ContainsKey(functionCallExpression.FunctionName))
        {
            throw new Exception($"!FUNCTION ERROR : Function {functionCallExpression.FunctionName} is not defined");
        }
        var functionDeclaration = StandardLibrary.Functions[functionCallExpression.FunctionName];
        if (functionDeclaration?.Arguments.Count != functionCallExpression.Parameters.Count)
        {
            throw new Exception($"!FUNCTION ERROR : Function {functionCallExpression.FunctionName} does not have {functionCallExpression.Parameters.Count} parameters but {StandardLibrary.Functions[functionCallExpression.FunctionName]?.Arguments.Count} parameters");
        }

        var parameters = functionCallExpression.Parameters;
        var arguments = functionDeclaration.Arguments;

        var functionCallScope = new Scope();
        foreach (var (arg, param) in arguments.Zip(parameters))
        {
            var evaluatedParameter = EvaluateExpression(param, scope.BuildChildScope());
            functionCallScope.AddVariable(arg, evaluatedParameter);
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
        var evaluateLetExpression = EvaluateExpression(letExpression.Expression, scope.BuildChildScope());
        scope.AddVariable(letExpression.Identifier.Text, evaluateLetExpression);
        if (letExpression.LetChildExpression is null)
        {
            return scope;
        }
        return EvaluateLetExpression(letExpression.LetChildExpression, scope.BuildChildScope());
    }

    private object EvaluateIfElseStatement(If_ElseStatement ifElseStatement, Scope scope)
    {
        var condition = EvaluateExpression(ifElseStatement.IfCondition, scope.BuildChildScope());
        bool boolCondition = false;
        boolCondition = CheckBooleanType(condition);
        if (boolCondition)
        {
            return EvaluateExpression(ifElseStatement.IfStatement, scope.BuildChildScope());
        }
        else
        {
            return EvaluateExpression(ifElseStatement.ElseClause, scope.BuildChildScope());
        }
    }
    private static bool CheckBooleanType(object condition)
    {
        bool boolCondition;
        if (condition.GetType() == typeof(bool))
        {
            boolCondition = (bool)condition;
        }
        else
        {
            throw new Exception("! SEMANTIC ERROR : If-ELSE expressions must have a boolean condition");
        }
        return boolCondition;
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
            if ((double)value == 0) return value;
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
                CheckTypes(binaryExpression, left, right);
                return (double)left + (double)right;
            case TokenType.MinusToken:
                CheckTypes(binaryExpression, left, right);
                return (double)left - (double)right;
            case TokenType.MultiplyToken:
                CheckTypes(binaryExpression, left, right);
                return (double)left * (double)right;
            case TokenType.DivisionToken:
                if ((double)right == 0)
                    throw new Exception("! SEMANTIC ERROR : Cannot divide by zero");
                CheckTypes(binaryExpression, left, right);
                return (double)left / (double)right;
            case TokenType.ModuleToken:
                CheckTypes(binaryExpression, left, right);
                return (double)left % (double)right;
            case TokenType.ArrobaToken:
                return left.ToString() + right.ToString();
            case TokenType.SingleAndToken:
                CheckTypes(binaryExpression, left, right);
                return (bool)left && (bool)right;
            case TokenType.SingleOrToken:
                CheckTypes(binaryExpression, left, right);
                return (bool)left || (bool)right;
            case TokenType.BiggerToken:
                CheckTypes(binaryExpression, left, right);
                return (double)left > (double)right;
            case TokenType.BiggerOrEqualToken:
                CheckTypes(binaryExpression, left, right);
                return (double)left >= (double)right;
            case TokenType.LowerToken:
                CheckTypes(binaryExpression, left, right);
                return (double)left < (double)right;
            case TokenType.LowerOrEqualToken:
                CheckTypes(binaryExpression, left, right);
                return (double)left <= (double)right;
            case TokenType.EqualToken:
                return Equals(left, right);
            case TokenType.NotEqualToken:
                return !Equals(left, right);
            case TokenType.ExponentialToken:
                CheckTypes(binaryExpression, left, right);
                return Pow((double)left, (double)right);
            default:
                throw new Exception($"! SEMANTIC ERROR : Unexpected binary operator {binaryExpression.OperatorToken.Type}");
        }
    }
    private object EvaluateLiteralExpression(HulkLiteralExpression literal, Scope scope)
    {
        if (literal.LiteralToken.Type is TokenType.Identifier)
        {
            return scope.GetValue(literal.LiteralToken.Text);
        }
        return literal.Value;
    }
    private double Pow(double left, double right)
    {
        if (left == 0 && right == 0) throw new Exception($"!SEMANTIC ERROR : {left} pow to {right} is not defined");
        return Math.Pow(left, right);
    }
    private static void CheckTypes(HulkBinaryExpression binaryExpression, object left, object right)
    {
        if (left.GetType() != right.GetType())
        {
            throw new Exception($"!SEMANTIC ERROR : Invalid expression: Can't operate {left.GetType()} with {right.GetType()} using {binaryExpression.OperatorToken.Text}");
        }
    }
}