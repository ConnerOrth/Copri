﻿using System.Collections.Generic;
using System.Linq;

namespace Copri.CodeAnalysis
{
    public sealed class SyntaxTree
    {
        public IReadOnlyList<string> Diagnostics { get; }
        public ExpressionSyntax Root { get; }
        public SyntaxToken EndOfFileToken { get; }

        public SyntaxTree(IEnumerable<string> diagnostics, ExpressionSyntax root, SyntaxToken endOfFileToken)
            => (Diagnostics, Root, EndOfFileToken) = (diagnostics.ToList(), root, endOfFileToken);

        public static SyntaxTree Parse(string text)
        {
            Parser parser = new Parser(text);
            return parser.Parse();
        }
    }
}