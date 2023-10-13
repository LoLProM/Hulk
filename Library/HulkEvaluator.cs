namespace HulkProject;

public class HulkEvaluator
{
    private HulkExpression _node;
    public HulkEvaluator(HulkExpression node)
    {
        this._node = node;
    }
    public object Evaluate()
    {
        return EvaluateExpression(_node);
    }
    private object EvaluateExpression(HulkExpression node)
    { 
        switch (node)
        {
            case HulkLiteralExpression literal:
                return EvaluateLiteralExpression(literal);
            case HulkUnaryExpression unary:
                return EvaluateUnaryExpression(unary);
            case HulkBinaryExpression binaryExpression:
                return EvaluateBinaryExpression(binaryExpression);
            case HulkParenthesesExpression parenthesesExpression:
                return EvaluateParenthesesExpression(parenthesesExpression);
            case If_ElseStatement ifElseStatement:
                return EvaluateIfElseStatement(ifElseStatement); 
            case Let_In_Expression letInExpression:
                return EvaluateLetInExpression(letInExpression);
            case PrintDeclaration printDeclaration:
                return EvaluatePrintDeclaration(printDeclaration);
            case FunctionCallExpression functionCallExpression:
                return EvaluateFunctionCallExpression(functionCallExpression);
        }
        throw new Exception($"Unexpected node {node}");
    }

    private object EvaluatePrintDeclaration(PrintDeclaration printDeclaration)
    {
        return EvaluateExpression(printDeclaration.ToPrintExpression);
    }

    private object EvaluateFunctionCallExpression(FunctionCallExpression functionCallExpression)
    {
        return true;
    }

    private object EvaluateLetInExpression(Let_In_Expression letInExpression)
    {
        return EvaluateExpression(letInExpression.InExpression);
    }

    private object EvaluateLiteralExpression(HulkLiteralExpression literal)
    {
        return literal.Value;
    }

    private object EvaluateParenthesesExpression(HulkParenthesesExpression parenthesesExpression)
    {
        return EvaluateExpression(parenthesesExpression.InsideExpression);
    }

    private object EvaluateUnaryExpression(HulkUnaryExpression unary)
    {
        var value = EvaluateExpression(unary.InternalExpression);
        if (unary.OperatorToken.Type == TokenType.MinusToken)
        {
            return -(int)value;
        }
        else if (unary.OperatorToken.Type == TokenType.PlusToken)
        {
            return value;
        }
        else if (unary.OperatorToken.Type == TokenType.NotToken)
        {
            return !(bool)value;
        }
        throw new Exception("Invalid unary operator");
    }
    private object EvaluateBinaryExpression(HulkBinaryExpression binaryExpression)
    {
        var left = EvaluateExpression(binaryExpression.Left);
        var right = EvaluateExpression(binaryExpression.Right);

        switch (binaryExpression.OperatorToken.Type)
        {
            case TokenType.PlusToken:
                return (int)left + (int)right;
            case TokenType.MinusToken:
                return (int)left - (int)right;
            case TokenType.MultiplyToken:
                return (int)left * (int)right;
            case TokenType.DivisionToken:
                if ((int)right == 0)
                    throw new InvalidOperationException("Cannot divide by zero");
                return (int)left / (int)right;
            case TokenType.ModuleToken:
                return (int)left % (int)right;
            case TokenType.SingleAndToken:
                return (bool)left && (bool)right;
            case TokenType.SingleOrToken:
                return (bool)left || (bool)right;
            case TokenType.BiggerToken:
                return (int)left > (int)right;
            case TokenType.BiggerOrEqualToken:
                return (int)left >= (int)right;
            case TokenType.LowerToken:
                return (int)left < (int)right;
            case TokenType.LowerOrEqualToken:
                return (int)left <= (int)right;
            case TokenType.EqualToken:
                return Equals(left, right);
            case TokenType.NotEqualToken:
                return !Equals(left, right);
            case TokenType.ExponentialToken:
                return Pow((int)left, (int)right);
            case TokenType.SingleEqualToken:
                var a = (object)left;
                return a = (object)right;
            default:
                throw new Exception($"Unexpected binary operator {binaryExpression.OperatorToken.Type}");
        }
    }
    private object EvaluateIfElseStatement(If_ElseStatement ifElseStatement)
    {
        var condition = (bool)EvaluateExpression(ifElseStatement.IfCondition);
        if (condition)
        {
            return EvaluateExpression(ifElseStatement.IfStatement);
        }
        else
        {
            return EvaluateExpression(ifElseStatement.ElseClause);
        }
    }
    private int Pow(int left, int right)
    {
        if (left == 0 && right == 0) throw new InvalidOperationException($"{left} pow to {right} is not defined");
        return (int)Math.Pow(left, right);
    }
}