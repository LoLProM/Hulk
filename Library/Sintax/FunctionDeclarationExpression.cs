namespace HulkProject;

public class FunctionDeclarationExpression : HulkExpression
//Esta clase es la representacion de la declaracion de una funcion su nombre, la lista de parametros y el cuerpo de la funcion
{
    public FunctionDeclarationExpression(string functionName, List<string> parameters, HulkExpression functionBody)
    {
        FunctionName = functionName;
        Arguments = parameters;
        FunctionBody = functionBody;
    }
    public string FunctionName { get; }
    public List<string> Arguments { get; }
    public HulkExpression FunctionBody { get; private set;}
    
}
