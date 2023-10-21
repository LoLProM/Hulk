namespace HulkProject
{
    public class FunctionReference : HulkExpression
    {
        private Func<Scope, object> function;

        public FunctionReference (Func<Scope, object> function)
        {
            this.function = function;
        }
        public object Eval(Scope param)
        {
            return function(param);
        }
    }
}