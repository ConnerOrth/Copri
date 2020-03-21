using System;

namespace Copri.CodeAnalysis.Binding
{
    internal sealed class BoundUnaryExpression : BoundExpression
    {
        public override Type Type => Operand.Type;
        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
        public BoundUnaryOperatorKind OperatorKind { get; }
        public BoundExpression Operand { get; }


        public BoundUnaryExpression(BoundUnaryOperatorKind operatorKind, BoundExpression operand)
            => (OperatorKind, Operand) = (operatorKind, operand);
    }
}