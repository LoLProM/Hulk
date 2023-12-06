namespace HulkProject;

public class If_ElseStatement : HulkExpression
{
//Esta clase es la representacion de que es un If Else Expression
    public If_ElseStatement(Token ifKeyword, HulkExpression ifCondition, HulkExpression ifStatement, HulkExpression elseClause)
    {
        IfKeyword = ifKeyword;
        IfCondition = ifCondition;
        IfStatement = ifStatement;
        ElseClause = elseClause;
        ExpressionType = ifStatement.ExpressionType;
    }
    public TokenType Type => TokenType.IfElseExpression;
    public Token IfKeyword { get; }
    public HulkExpression IfCondition { get; }
    public HulkExpression IfStatement { get; }
    public HulkExpression ElseClause { get; }
}

