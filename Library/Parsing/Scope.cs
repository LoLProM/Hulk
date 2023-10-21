namespace HulkProject;

public class Scope
{
    protected Scope? parent;
    protected Scope? child;
    private Dictionary<string, object> variables;
    public Scope()
    {
        variables = new Dictionary<string, object>();
    }

    public Scope(List<string> parameters) : this()
    {
        foreach (var parameter in parameters)
        {
            variables.Add(parameter, null);
        }
    }
    public void AddVariable(string identifier, object expression)
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
    public object GetValue(string identifier)
    {
        if(variables.ContainsKey(identifier))
        {
            return variables[identifier];
        }
        return parent is null ? throw new Exception($"! SEMANTIC ERROR : Undefine variable {identifier}") : parent.GetValue(identifier);
    }
}