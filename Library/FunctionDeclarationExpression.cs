namespace HulkProject;

public class FunctionDeclarationExpression : HulkExpression
{
    public FunctionDeclarationExpression(string functionName, List<string> parameters, HulkExpression functionBody)
    {
        FunctionName = functionName;
        Arguments = parameters;
        FunctionBody = functionBody;
        ExpressionType = functionBody.ExpressionType;
    }
    public string FunctionName { get; }
    public List<string> Arguments { get; }
    public HulkExpression FunctionBody { get; }

    public override HulkExpression UseScope(Scope functionScope)
    {
        return this;
    }
}