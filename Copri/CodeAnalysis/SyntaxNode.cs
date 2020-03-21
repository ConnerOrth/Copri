using System.Collections.Generic;

namespace Copri.CodeAnalysis
{
    abstract class SyntaxNode
    {
        public abstract SyntaxKind Kind { get; }

        public abstract IEnumerable<SyntaxNode> GetChildren();
    }
}
