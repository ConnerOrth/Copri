using System.Collections.Generic;

namespace Copri.CodeAnalysis
{
    sealed class LiteralExpressionSyntax : ExpressionSyntax
    {
        public override SyntaxKind Kind => SyntaxKind.LiteralExpression;
        public SyntaxToken LiteralToken { get; }

        public LiteralExpressionSyntax(SyntaxToken literalToken) => LiteralToken = literalToken;

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return LiteralToken;
        }
    }
}
