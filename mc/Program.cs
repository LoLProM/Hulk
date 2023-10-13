namespace HulkProject;

class Program
{
    static string[] inputs = {
        "function Sum(x,y) => x + y;",
        // "function Sum(x,y,z) => x + y + z;",
        "Sum(1,2);",
        // "Mult(1,2);"
    };
    public static void Main(string[] args)
    {

        // foreach (var input in inputs)
        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine();
            // var input = "a2;";
            // Console.WriteLine($"> {input}");
            try
            {
                if (string.IsNullOrEmpty(input))
                {
                    return;
                }
                var lexer = new Lexer(input);
                var tree = ASTree.Parse(input);
                if (tree.Root is FunctionDeclarationExpression)
                {
                    continue;
                }
                var e = new HulkEvaluator(tree.Root);
                var result = e.Evaluate();

                Console.WriteLine();
                Console.WriteLine($"EL Resultado es {result}");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}