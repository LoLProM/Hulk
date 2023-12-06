namespace HulkProject
{
    class ASTree //Arbol que devuelve el parser
    {
        public ASTree(HulkExpression root, Token endOfLineToken)
        {
            Root = root;
            EndOfLineToken = endOfLineToken;
        }
        public HulkExpression Root { get; }
        public Token EndOfLineToken { get; }
        public static ASTree Parse(string text)
        {
            var parser = new Parser(text);
            return parser.Parse();  
        }
    }
}