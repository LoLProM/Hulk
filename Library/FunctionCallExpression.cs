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
}
