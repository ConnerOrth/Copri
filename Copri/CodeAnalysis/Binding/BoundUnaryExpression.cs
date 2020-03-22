using System;

namespace Copri.CodeAnalysis.Binding
{
    internal sealed class BoundUnaryExpression : BoundExpression
    {
        public override Type Type => Operator.Type;
        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
        public BoundUnaryOperator Operator { get; }
        public BoundExpression Operand { get; }


        public BoundUnaryExpression(BoundUnaryOperator @operator, BoundExpression operand)
            => (Operator, Operand) = (@operator, operand);
    }
}