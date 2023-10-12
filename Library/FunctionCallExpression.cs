namespace HulkProject;

public class FunctionCallExpression : HulkExpression
{
    public FunctionCallExpression(string functionName, List<HulkExpression> parameters)
    {
        FunctionName = functionName;
        Parameters = parameters;
    }

    public string FunctionName { get; }
    public List<HulkExpression> Parameters { get; }

    public override HulkExpression UseScope(Scope functionScope)
    {
        var scopedParameters = Parameters.Select(p => p.UseScope(functionScope)).ToList();
        return new FunctionCallExpression(FunctionName, scopedParameters);
    }
}
