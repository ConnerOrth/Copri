using System;

namespace Copri.CodeAnalysis.Binding
{
    internal sealed class BoundBinaryExpression : BoundExpression
    {
        public override Type Type => Operator.Type;
        public override BoundNodeKind Kind => BoundNodeKind.BinaryExpression;
        public BoundExpression Left { get; }
        public BoundBinaryOperator Operator { get; }
        public BoundExpression Right { get; }


        public BoundBinaryExpression(BoundExpression left, BoundBinaryOperator @operator, BoundExpression right)
            => (Left, Operator, Right) = (left, @operator, right);
    }
}