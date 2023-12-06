namespace HulkProject;

public class Scope
//Clase encargada de tener el ambito del programa
//Tenemos el diccionario de variables 
{
    protected Scope? parent;
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
    public bool Contains(string identifier)//Verificar que el identificador pertenece sino pertenece buscamos en el padre sino esta tampoco entonces no ha sido guardada la variable
    {
        if(variables.ContainsKey(identifier))
        {
            return true;
        }
        return parent is null ? false : parent.Contains(identifier);
    }
    public Scope BuildChildScope()
    //Creamos un ambito nuevo para cada operacion que hagamos en el evaluador y poder tener cada variable en un scope hijo y el padre poder tener acceso a todas ellas
    {
        var newScope = new Scope{
            parent = this
        };
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