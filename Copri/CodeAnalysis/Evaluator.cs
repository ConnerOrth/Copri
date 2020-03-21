﻿using Copri.CodeAnalysis.Binding;
using Copri.CodeAnalysis.Syntax;
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

        public int Evaluate()
        {
            return EvaluateExpression(root);
        }

        private int EvaluateExpression(BoundExpression node)
        {
            if (node is BoundLiteralExpression l)
            {
                return (int)l.Value;
            }

            if (node is BoundUnaryExpression u)
            {
                int operand = EvaluateExpression(u.Operand);
                return u.OperatorKind switch
                {
                    BoundUnaryOperatorKind.Identity => operand,
                    BoundUnaryOperatorKind.Negation => -operand,
                    _ => throw new Exception($"Unexpected unary operator {u.OperatorKind}."),
                };
            }

            if (node is BoundBinaryExpression b)
            {
                int left = EvaluateExpression(b.Left);
                int right = EvaluateExpression(b.Right);

                return b.OperatorKind switch
                {
                    BoundBinaryOperatorKind.Addition => left + right,
                    BoundBinaryOperatorKind.Subtraction => left - right,
                    BoundBinaryOperatorKind.Multiplication => left * right,
                    BoundBinaryOperatorKind.Division => left / right,
                    _ => throw new Exception($"Unexpected binary operator {b.OperatorKind}."),
                };
            }
            
            throw new Exception($"Unexpected node {node.Kind}.");
        }
    }
}
