namespace HulkProject;

public static class StandardLibrary
{
    public static Dictionary<string, FunctionDeclarationExpression?> Functions = StandardLibrary.GetStandardLibraryFunctions()!;

    public static Scope Variables = GetStandardLibraryVariables();

    private static Scope GetStandardLibraryVariables()
    {
        var variables = new Scope();
        variables.AddVariable("PI", Math.PI);
        variables.AddVariable("E", Math.E);
        return variables;
    }
    
    private static Dictionary<string, FunctionDeclarationExpression> GetStandardLibraryFunctions()
    {
        return new()
        {
            ["sin"] = new FunctionDeclarationExpression("sin", new() { "x" }, new FunctionReference((scope) => Math.Sin((double)scope.GetValue("x")))),
            
            ["cos"] = new FunctionDeclarationExpression("cos", new() { "x" }, new FunctionReference((scope) => Math.Cos((double)scope.GetValue("x")))),

            ["sqrt"] = new FunctionDeclarationExpression("sqrt", new() { "x" }, new FunctionReference((scope) => Math.Sqrt((double)scope.GetValue("x")))),

            ["log"] = new FunctionDeclarationExpression("log", new() { "b", "p" }, new FunctionReference((scope) => Math.Log((double)scope.GetValue("p"), (double)scope.GetValue("b")))),

            ["log2"] = new FunctionDeclarationExpression("log2", new() { "p" }, new FunctionReference((scope) => Math.Log2((double)scope.GetValue("p")))),

            ["log10"] = new FunctionDeclarationExpression("log10", new() { "p" }, new FunctionReference((scope) => Math.Log10((double)scope.GetValue("p")))),

            ["print"] = new FunctionDeclarationExpression("print", new() { "x" }, new FunctionReference((scope) => scope.GetValue("x").ToString()!)),
        };
    }
}