namespace HulkProject
{
    public class LetExpression : HulkExpression
    {
        public LetExpression(Token identifier, HulkExpression expression) : this(identifier, expression, null!)
        {
        }

        public LetExpression(Token identifier, HulkExpression expression, LetExpression letChildExpression)
        {
            Identifier = identifier;
            Expression = expression;
            LetChildExpression = letChildExpression;
            ExpressionType = Expression.ExpressionType;
        }
        public Token Identifier { get; }
        public HulkExpression Expression { get; }
        public LetExpression LetChildExpression { get; }

        public override HulkExpression UseScope(Scope functionScope)
        {
            return new LetExpression(Identifier, 
            Expression.UseScope(functionScope),
            (LetExpression)LetChildExpression?.UseScope(functionScope)
            );
        }
    }
}