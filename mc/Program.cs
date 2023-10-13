﻿namespace HulkProject;
using System.Text.RegularExpressions;

class Program
{
    static string[] inputs = {
        "function Sum(x,y) => x + y;",
        // "function Sum(x,y,z) => x + y + z;",
        "Sum(1,2);",
        "Mult(1,2);"
    };
    public static void Main(string[] args)
    {

        // foreach (var input in inputs)
        while (true)
        {
            Console.Write("> ");
            // var input = Console.ReadLine();
            var input = "let a = 2 in let b = a * 2 in b + 1;";
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
                System.Console.WriteLine($"EL Resultado es {result}");
            }
            catch(Exception e)
            {
                
                Console.WriteLine(e.Message);
            }

        }
    }
}