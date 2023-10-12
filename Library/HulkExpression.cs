namespace HulkProject;

public abstract class HulkExpression
{
    public virtual Type ExpressionType { get; init;}

    public abstract HulkExpression UseScope(Scope functionScope);

}