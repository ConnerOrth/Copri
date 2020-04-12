using Copri.CodeAnalysis.Binding;
using Copri.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Copri.CodeAnalysis
{
    public class Compilation
    {
        public SyntaxTree Syntax { get; }

        public Compilation(SyntaxTree syntax)
        {
            Syntax = syntax;
        }

        public EvaluationResult Evaluate()
        {
            Binder binder = new Binder();
            BoundExpression boundExpression = binder.BindExpression(Syntax.Root);

            IList<Diagnostic> diagnostics = Syntax.Diagnostics.Concat(binder.Diagnostics).ToList();

            if (diagnostics.Any()) return new EvaluationResult(diagnostics, null);

            Evaluator evaluator = new Evaluator(boundExpression);
            object value = evaluator.Evaluate();

            return new EvaluationResult(Array.Empty<Diagnostic>(), value);
        }
    }
}
