namespace HulkProject;
//Clase padre del Hulk todas las expressiones heredan de HulkExpression y por tanto todas tienen un type
public abstract class HulkExpression
{
    public virtual Type ExpressionType { get; init;}

}