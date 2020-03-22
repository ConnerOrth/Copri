using Copri.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Copri.CodeAnalysis.Binding
{
    internal sealed class BoundUnaryOperator
    {
        private static readonly IList<BoundUnaryOperator> operators = new List<BoundUnaryOperator>()
        {
            new BoundUnaryOperator(SyntaxKind.BangToken, BoundUnaryOperatorKind.LogicalNegation, typeof(bool)),

            new BoundUnaryOperator(SyntaxKind.PlusToken, BoundUnaryOperatorKind.Identity, typeof(int)),
            new BoundUnaryOperator(SyntaxKind.MinusToken, BoundUnaryOperatorKind.Negation, typeof(int)),
        };

        public SyntaxKind SyntaxKind { get; }
        public BoundUnaryOperatorKind Kind { get; }
        public Type OperandType { get; }
        public Type ReturnType { get; }

        private BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryOperatorKind kind, Type operandType) : this(syntaxKind, kind, operandType, operandType)
        { }
        private BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryOperatorKind kind, Type operandType, Type returnType)
            => (SyntaxKind, Kind, OperandType, ReturnType) = (syntaxKind, kind, operandType, returnType);

        public static BoundUnaryOperator Bind(SyntaxKind syntaxKind, Type operandType)
            => operators.FirstOrDefault(o => o.SyntaxKind == syntaxKind && o.OperandType == operandType);
    }
}