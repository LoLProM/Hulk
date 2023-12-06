namespace HulkProject;

public class HulkBinaryExpression : HulkExpression
//Esta clase es la representacion de la Expresion Binaria que tiene una parte izquierda una derecha y un operador el type de la expresion devuelve si la izquierda y la derecha son de un tipo en especifico dependiendo del operador si tenemos un operador booleano y la izquierda y derecha son numeros el tipo de la expression binaria es booleano
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
            else if (Left.ExpressionType == typeof(double) && Right.ExpressionType == typeof(double))
            {
                return typeof(double);
            }
            else
            {
                return null;
            }
        }

        else if (OperatorToken.Type is TokenType.MinusToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }
            else if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(double))
            {
                return typeof(double);
            }
            else
            {
                return null;
            }
        }

        else if (OperatorToken.Type is TokenType.MultiplyToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }
            else if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(double))
            {
                return typeof(double);
            }
            else
            {
                return null;
            }
        }

        else if (OperatorToken.Type is TokenType.DivisionToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }
            else if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(double))
            {
                return typeof(double);
            }
            else
            {
                return null;
            }
        }

        else if (OperatorToken.Type is TokenType.SingleAndToken)
        {

            if (Left == null && Right == null)
            {
                return null!;
            }
            else if (Left.ExpressionType == typeof(bool) && Right.ExpressionType == typeof(bool))
            {
                return typeof(bool);
            }
            else
            {
                return null;
            }
        }
        else if (OperatorToken.Type is TokenType.SingleOrToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }
            else if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(bool))
            {
                return typeof(bool);
            }
            else
            {
                return null;
            }
        }
        else if (OperatorToken.Type is TokenType.ModuleToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }

            else if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(double))
            {
                return typeof(double);
            }
            else
            {
                return null;
            }
        }
        else if (OperatorToken.Type is TokenType.BiggerToken || OperatorToken.Type is TokenType.BiggerOrEqualToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }

            else if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(double))
                return typeof(bool);
            else
            {
                return null;
            }
        }

        else if (OperatorToken.Type is TokenType.LowerToken || OperatorToken.Type is TokenType.LowerOrEqualToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }

            else if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(double))
                return typeof(bool);
            else
            {
                return null;
            }
        }

        else if (OperatorToken.Type is TokenType.EqualToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }

            else if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(double))
                return typeof(bool);
            else
            {
                return null;
            }

        }
        else if (OperatorToken.Type is TokenType.NotEqualToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }

            else if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(double))
            {
                return typeof(bool);
            }
            else
            {
                return null;
            }

        }
        else if (OperatorToken.Type is TokenType.ExponentialToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }

            else if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(double))
            {
                return typeof(double);
            }
            else
            {
                return null;
            }
        }
        else if (OperatorToken.Type is TokenType.SingleEqualToken)
        {
            if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(double))
            {
                return typeof(double);
            }
            else if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(bool))
            {
                return typeof(bool);
            }
            else
            {
                return null;
            }
        }
        else if (OperatorToken.Type is TokenType.ArrobaToken)
        {
            if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(string))
            {
                return typeof(string);
            }
            return null;
        }
        throw new Exception($"!SEMANTIC ERROR : Invalid expression: Can't operate {Left.ExpressionType} with {Right.ExpressionType} using {OperatorToken.Text}");
    }
}

