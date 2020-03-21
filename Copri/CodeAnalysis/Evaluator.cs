using System;

namespace Copri.CodeAnalysis
{
    class Evaluator
    {
        private readonly ExpressionSyntax root;

        public Evaluator(ExpressionSyntax root)
        {
            this.root = root;
        }

        public int Evaluate()
        {
            return EvaluateExpression(root);
        }

        private int EvaluateExpression(ExpressionSyntax node)
        {
            if (node is LiteralExpressionSyntax l)
            {
                return (int)l.LiteralToken.Value;
            }
            if (node is BinaryExpressionSyntax b)
            {
                int left = EvaluateExpression(b.Left);
                int right = EvaluateExpression(b.Right);

                if (b.OperatorToken.Kind == SyntaxKind.PlusToken) return left + right;
                else if (b.OperatorToken.Kind == SyntaxKind.MinusToken) return left - right;
                else if (b.OperatorToken.Kind == SyntaxKind.StarToken) return left * right;
                else if (b.OperatorToken.Kind == SyntaxKind.SlashToken) return left / right;
                throw new Exception($"Unexpected binary operator {b.OperatorToken.Kind}.");
            }
            if (node is ParenthesizedExpressionSyntax p)
            {
                return EvaluateExpression(p.Expression);
            }
            throw new Exception($"Unexpected node {node.Kind}.");
        }
    }
}
