namespace HulkProject;

public class If_ElseStatement : HulkExpression
{
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

    public override HulkExpression UseScope(Scope functionScope)
    {
        return new If_ElseStatement(
            IfKeyword,
        IfCondition.UseScope(functionScope),
        IfStatement.UseScope(functionScope),
        ElseClause.UseScope(functionScope)
        );
    }
}

