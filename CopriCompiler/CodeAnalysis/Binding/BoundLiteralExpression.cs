using System;

namespace Copri.CodeAnalysis.Binding
{
    internal sealed class BoundLiteralExpression : BoundExpression
    {
        public override Type Type => Value.GetType();
        public override BoundNodeKind Kind => BoundNodeKind.LiteralExpression;
        public object Value { get; }

        public BoundLiteralExpression(object value) => Value = value;
    }
}