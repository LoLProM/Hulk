namespace HulkProject;

class Program
{
    static string[] inputs = {
        "2*2+2/2;",
        "function fact(x) => if (x == 1) 1 else x * fact(x-1);",
        "fact(5);",
        "function Sum(x,y) => x + y;",
        // "function Sum(x,y,z) => x + y + z;",
        "Sum(1,2);",
        "Mult(1,2);",
        "let a = 2 in a = 3;",
        "let a = let b = 2 in b in a *2;",
        "let a = if (let b = 2 in b == --(2*1)) 3 else 1 in a *4;"
    };
    public static void Main(string[] args)
    {

        foreach (var input in inputs)
        // while (true)
        {
            // Console.Write("> ");
            // var input = Console.ReadLine();
            // var input = "function Sum(x,y) => x + y;";
            Console.WriteLine($"> {input}");
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