using Copri.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Copri.CodeAnalysis.Binding
{
    internal sealed class BoundBinaryOperator
    {
        private static readonly IList<BoundBinaryOperator> operators = new List<BoundBinaryOperator>()
        {
            new BoundBinaryOperator(SyntaxKind.AmpersandAmpersandToken, BoundBinaryOperatorKind.LogicalAnd, typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.PipePipeToken, BoundBinaryOperatorKind.LogicalOr, typeof(bool)),

            new BoundBinaryOperator(SyntaxKind.PlusToken, BoundBinaryOperatorKind.Addition, typeof(int)),
            new BoundBinaryOperator(SyntaxKind.MinusToken, BoundBinaryOperatorKind.Subtraction, typeof(int)),
            new BoundBinaryOperator(SyntaxKind.StarToken, BoundBinaryOperatorKind.Multiplication, typeof(int)),
            new BoundBinaryOperator(SyntaxKind.SlashToken, BoundBinaryOperatorKind.Division, typeof(int)),
        };

        public SyntaxKind SyntaxKind { get; }
        public BoundBinaryOperatorKind Kind { get; }
        public Type LeftType { get; }
        public Type RightType { get; }
        public Type ReturnType { get; }

        private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type type) : this(syntaxKind, kind, type, type,type)
        { }
        private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type leftType, Type rightType, Type returnType)
            => (SyntaxKind, Kind, LeftType, RightType, ReturnType) = (syntaxKind, kind, leftType, rightType, returnType);

        public static BoundBinaryOperator Bind(SyntaxKind syntaxKind, Type leftType, Type rightType)
            => operators.FirstOrDefault(o => o.SyntaxKind == syntaxKind && o.LeftType == leftType && o.RightType == rightType);
    }
}