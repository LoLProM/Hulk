namespace HulkProject;

public class Scope
{
    protected Scope? parent;

    protected Scope? child;
    private Dictionary<string, HulkExpression?> variables;

    public static Scope EmptyScope => new Scope();
    
    public Scope()
    {
        variables = new Dictionary<string, HulkExpression?>();
    }

    public Scope(List<string> parameters) : this()
    {
        foreach (var parameter in parameters)
        {
            variables.Add(parameter, null);
        }
    }
    public void AddVariable(string identifier, HulkExpression expression)
    {
        variables[identifier] = expression;
    }
    public bool Contains(string identifier)
    {
        if(variables.ContainsKey(identifier))
        {
            return true;
        }
        return parent is null ? false : parent.Contains(identifier);
    }
    public Scope BuildChildScope()
    {
        var newScope = new Scope();
        child = newScope;
        newScope.parent = this;
        return newScope;
    }

    public Scope BottomScope()
    {
        var scope = this;
        while (scope.child is not null)
        {
            scope = scope.child;
        }
        return scope;
    }
    public HulkExpression GetExpression(string identifier)
    {
        if(variables.ContainsKey(identifier))
        {
            return variables[identifier];
        }
        return parent is null ? throw new Exception("Not identifier found") : parent.GetExpression(identifier);
    }
}