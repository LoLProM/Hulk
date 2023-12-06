namespace HulkProject;

public class FunctionCallExpression : HulkExpression
//Esta clase es la representacion de la llamada de una funcion su nombre y la lista de parametros
{
    public FunctionCallExpression(string functionName, List<HulkExpression> parameters)
    {
        FunctionName = functionName;
        Parameters = parameters;
    }

    public string FunctionName { get; }
    public List<HulkExpression> Parameters { get; }
}
