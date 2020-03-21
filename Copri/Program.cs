using Copri.CodeAnalysis;
using System;
using System.Linq;

namespace Copri
{
    internal static class Program
    {
        private static void Main()
        {
            bool showTree = false;
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) return;

                if (input.Equals("#showTree", StringComparison.OrdinalIgnoreCase))
                {
                    showTree = !showTree;
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine(showTree ? "Showing parse trees." : "Hiding parse trees.");
                    Console.ResetColor();
                    continue;
                }
                else if (input.Equals("#cls", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Clear();
                    continue;
                }


                SyntaxTree syntaxTree = SyntaxTree.Parse(input);

                if (showTree)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    PrettyPrint(syntaxTree.Root);
                    Console.ResetColor();
                }

                if (!syntaxTree.Diagnostics.Any())
                {
                    Evaluator evaluator = new Evaluator(syntaxTree.Root);
                    int result = evaluator.Evaluate();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(result);
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;

                    foreach (string diagnostic in syntaxTree.Diagnostics)
                    {
                        Console.WriteLine(diagnostic);
                    }

                    Console.ResetColor();
                }
            }
        }

        static void PrettyPrint(SyntaxNode node, string indent = "", bool isLast = true)
        {
            string marker = isLast ? "└──" : "├──";

            Console.Write(indent);
            Console.Write(marker);
            Console.Write(node.Kind);

            if (node is SyntaxToken t && t.Value is object)
            {
                Console.Write(" ");
                Console.Write(t.Value);
            }

            Console.WriteLine();

            indent += isLast ? "   " : "│  ";

            SyntaxNode lastChild = node.GetChildren().LastOrDefault();
            foreach (SyntaxNode child in node.GetChildren())
            {
                PrettyPrint(child, indent, child == lastChild);
            }
        }
    }
}
