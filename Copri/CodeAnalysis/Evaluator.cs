using Copri.CodeAnalysis.Binding;
using System;

namespace Copri.CodeAnalysis
{
    internal class Evaluator
    {
        private readonly BoundExpression root;

        public Evaluator(BoundExpression root)
        {
            this.root = root;
        }

        public object Evaluate()
        {
            return EvaluateExpression(root);
        }

        private object EvaluateExpression(BoundExpression node)
        {
            if (node is BoundLiteralExpression l)
            {
                return l.Value;
            }

            if (node is BoundUnaryExpression u)
            {
                object operand = EvaluateExpression(u.Operand);
                return u.Operator.Kind switch
                {
                    BoundUnaryOperatorKind.Identity => (int)operand,
                    BoundUnaryOperatorKind.Negation => -(int)operand,
                    BoundUnaryOperatorKind.LogicalNegation => !(bool)operand,
                    _ => throw new Exception($"Unexpected unary operator {u.Operator}."),
                };
            }

            if (node is BoundBinaryExpression b)
            {
                object left = EvaluateExpression(b.Left);
                object right = EvaluateExpression(b.Right);

                return b.Operator.Kind switch
                {
                    BoundBinaryOperatorKind.Addition => (int)left + (int)right,
                    BoundBinaryOperatorKind.Subtraction => (int)left - (int)right,
                    BoundBinaryOperatorKind.Multiplication => (int)left * (int)right,
                    BoundBinaryOperatorKind.Division => (int)left / (int)right,
                    BoundBinaryOperatorKind.LogicalAnd => (bool)left && (bool)right,
                    BoundBinaryOperatorKind.LogicalOr => (bool)left || (bool)right,
                    BoundBinaryOperatorKind.Equals => Equals(left, right),
                    BoundBinaryOperatorKind.NotEquals => !Equals(left, right),
                    _ => throw new Exception($"Unexpected binary operator {b.Operator}."),
                };
            }

            throw new Exception($"Unexpected node {node.Kind}.");
        }
    }
}
