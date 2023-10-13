namespace HulkProject;

public class HulkBinaryExpression : HulkExpression
{
    public HulkBinaryExpression(HulkExpression left, Token operatorToken, HulkExpression right)
    {
        Left = left;
        OperatorToken = operatorToken;
        Right = right;
        ExpressionType = GetExpressionType();
    }
    public HulkExpression Left { get; }
    public Token OperatorToken { get; }
    public HulkExpression Right { get; }
    public Type GetExpressionType()
    {
        if (OperatorToken.Type is TokenType.PlusToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }
            else if (Left.ExpressionType == typeof(int) && Right.ExpressionType == typeof(int))
            {
                return typeof(int);
            }
            if (Left is HulkBinaryExpression p)
            {
                if (p.Left == null && p.Right == null)
                {
                    return null!;
                }
            }
        }

        else if (OperatorToken.Type is TokenType.MinusToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }
            if (Left is HulkBinaryExpression p)
            {
                if (p.Left == null && p.Right == null)
                {
                    return null!;
                }
            }
            else if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(int))
            {
                return typeof(int);
            }

        }

        else if (OperatorToken.Type is TokenType.MultiplyToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }
            if (Left is HulkBinaryExpression p)
            {
                if (p.Left == null && p.Right == null)
                {
                    return null!;
                }
            }
            else if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(int))
            {
                return typeof(int);
            }
        }

        else if (OperatorToken.Type is TokenType.DivisionToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }
            if (Left is HulkBinaryExpression p)
            {
                if (p.Left == null && p.Right == null)
                {
                    return null!;
                }
            }
            else if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(int))
            {
                return typeof(int);
            }
        }

        else if (OperatorToken.Type is TokenType.SingleAndToken)
        {

            if (Left == null && Right == null)
            {
                return null!;
            }
            if (Left is HulkBinaryExpression p)
            {
                if (p.Left == null && p.Right == null)
                {
                    return null!;
                }
            }
            else if (Left.ExpressionType == typeof(bool) && Right.ExpressionType == typeof(bool))
            {
                return typeof(bool);
            }
        }
        else if (OperatorToken.Type is TokenType.SingleOrToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }
            if (Left is HulkBinaryExpression p)
            {
                if (p.Left == null && p.Right == null)
                {
                    return null!;
                }
            }
            else if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(bool))
            {
                return typeof(bool);
            }

        }
        else if (OperatorToken.Type is TokenType.ModuleToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }
            if (Left is HulkBinaryExpression p)
            {
                if (p.Left == null && p.Right == null)
                {
                    return null!;
                }
            }
            else if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(int))
            {
                return typeof(int);
            }

        }
        else if (OperatorToken.Type is TokenType.BiggerToken || OperatorToken.Type is TokenType.BiggerOrEqualToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }
            if (Left is HulkBinaryExpression p)
            {
                if (p.Left == null && p.Right == null)
                {
                    return null!;
                }
            }
            else if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(int))
                return typeof(bool);
        }

        else if (OperatorToken.Type is TokenType.LowerToken || OperatorToken.Type is TokenType.LowerOrEqualToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }
            if (Left is HulkBinaryExpression p)
            {
                if (p.Left == null && p.Right == null)
                {
                    return null!;
                }
            }
            else if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(int))
                return typeof(bool);
        }

        else if (OperatorToken.Type is TokenType.EqualToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }
            if (Left is HulkBinaryExpression p)
            {
                if (p.Left == null && p.Right == null)
                {
                    return null!;
                }
            }
            else if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(int))
                return typeof(bool);

        }
        else if (OperatorToken.Type is TokenType.NotEqualToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }
            if (Left is HulkBinaryExpression p)
            {
                if (p.Left == null && p.Right == null)
                {
                    return null!;
                }
            }
            else if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(int))
            {
                return typeof(bool);
            }

        }
        else if (OperatorToken.Type is TokenType.ExponentialToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }
            if (Left is HulkBinaryExpression p)
            {
                if (p.Left == null && p.Right == null)
                {
                    return null!;
                }
            }
            else if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(int))
            {
                return typeof(int);
            }
        }
        else if (OperatorToken.Type is TokenType.SingleEqualToken)
        {
            if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(int))
            {
                return typeof(int);
            }
            else if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(bool))
            {
                return typeof(bool);
            }
            throw new Exception($"Cannot assign {Left.ExpressionType} to {Right.ExpressionType}");
        }
        throw new InvalidOperationException($"Invalid expression: Can't operate {Left.ExpressionType} with {Right.ExpressionType} using {OperatorToken.Text}");
    }

    public override HulkExpression UseScope(Scope functionScope)
    {
        return new HulkBinaryExpression(
            Left.UseScope(functionScope),
            OperatorToken,
            Right.UseScope(functionScope)
            );
    }
}

