namespace HulkProject;

class Program
{
    static string[] inputs = {
        // "2*2+2/2;",
        "function fact(x) => if (x == 1) 1 else x * fact(x-1);",
        "fact(14);",
        "function Sum(x,y) => x + y;",
        "function fib(x) => if (x == 1 | x == 0) 1 else fib(x-1) + fib(x-2);",
        "fib(\"string\");",
        "if (let a = 2 in a ==2 ) a else 1;",
        "function Sum(x,y,z) => x + y + z;",
        "Sum(1,2);",
        "Mult(1,2);",
        "let a = 2 in let a = true in a;",
        "let a = let b = 2 in b in a *2;",
        "let a = if (let b = 2 in b == --(2*1)) 3 else 1 in fact(2+a) * 2 /12;",
        "fib(7) * fact(3) + Sum(fib(3) + fact(5),fact(2)) + let a = 2 in a * 2;",
        "fact(let a = 2 in a + 3);",
        "fact(if (let a = if (2 < 3) 2 else 1 in fact(3+a) == fact(5)) 5 else 1);",
        "fib( Sum(fact(2+fact(1)),1));",
        "fact(sin(0)+cos(0)+4+E-E);"
    };
    public static void Main(string[] args)
    {

        foreach (var input in inputs)
        // while (true)
        {
            // Console.Write("> ");
            // var input = Console.ReadLine();
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
                System.Console.WriteLine();
                System.Console.WriteLine();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}