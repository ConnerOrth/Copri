using Copri.CodeAnalysis.Syntax;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Copri.CodeAnalysis
{
    internal sealed class DiagnosticBag : IEnumerable<Diagnostic>
    {
        private readonly List<Diagnostic> diagnostics = new List<Diagnostic>();

        public IEnumerator<Diagnostic> GetEnumerator() => diagnostics.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public void AddRange(DiagnosticBag diagnostics) => this.diagnostics.AddRange(diagnostics.diagnostics);

        private void Report(TextSpan textSpan, string message)
        {
            Diagnostic diagnostic = new Diagnostic(textSpan, message);
            diagnostics.Add(diagnostic);
        }

        public void ReportInvalidNumber(TextSpan textSpan, string text, Type type)
        {
            string message = $"The number '{text}' is not a valid {type}.";
            Report(textSpan, message);
        }

        public void ReportBadCharacter(int position, char character) 
        {
            TextSpan textSpan = new TextSpan(position, 1);
            string message = $"Bad character in input: '{character}'.";
            Report(textSpan, message);
        }

        public void ReportUnexpectedToken(TextSpan textSpan, SyntaxKind actualKind, SyntaxKind expectedKind)
        {
            string message = $"Unexpected token <{actualKind}>, expected <{expectedKind}>.";
            Report(textSpan, message);
        }

        public void ReportUndefinedUnaryOperator(TextSpan textSpan, string operatorText, Type operandType)
        {
            string message = $"Unary operator '{operatorText}' is not defined for type {operandType}.";
            Report(textSpan, message);
        }

        public void ReportUndefinedBinaryOperator(TextSpan textSpan, string operatorText, Type leftOperandType, Type rightOperandType)
        {
            string message = $"Binary operator '{operatorText}' is not defined for types {leftOperandType} and {rightOperandType}.";
            Report(textSpan, message);
        }

        public void ReportUndefinedName(TextSpan textSpan, string name)
        {
            string message = $"Variable '{name}' does not exist.";
            Report(textSpan, message);
        }
    }
}
