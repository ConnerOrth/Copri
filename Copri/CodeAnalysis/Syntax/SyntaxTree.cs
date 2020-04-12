using System.Collections.Generic;
using System.Linq;

namespace Copri.CodeAnalysis.Syntax
{
    public sealed class SyntaxTree
    {
        public IReadOnlyList<Diagnostic> Diagnostics { get; }
        public ExpressionSyntax Root { get; }
        public SyntaxToken EndOfFileToken { get; }

        public SyntaxTree(IEnumerable<Diagnostic> diagnostics, ExpressionSyntax root, SyntaxToken endOfFileToken)
            => (Diagnostics, Root, EndOfFileToken) = (diagnostics.ToList(), root, endOfFileToken);

        public static SyntaxTree Parse(string text)
        {
            Parser parser = new Parser(text);
            return parser.Parse();
        }
    }
}
