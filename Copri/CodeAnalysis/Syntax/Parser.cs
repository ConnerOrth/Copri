using System.Collections.Generic;

namespace Copri.CodeAnalysis.Syntax
{
    internal sealed class Parser
    {
        private readonly IList<SyntaxToken> tokens = new List<SyntaxToken>();
        private readonly DiagnosticBag diagnostics = new DiagnosticBag();
        private int position;

        public Parser(string text)
        {
            Lexer lexer = new Lexer(text);
            SyntaxToken token;
            do
            {
                token = lexer.Lex();
                if (token.Kind != SyntaxKind.WhiteSpaceToken &&
                    token.Kind != SyntaxKind.BadToken)
                {
                    tokens.Add(token);
                }
            } while (token.Kind != SyntaxKind.EndOfFileToken);

            diagnostics.AddRange(lexer.Diagnostics);
        }

        public DiagnosticBag Diagnostics => diagnostics;

        private SyntaxToken Peek(int offset)
        {
            int index = position + offset;

            if (index >= tokens.Count)
            {
                return tokens[^1];
            }
            return tokens[index];
        }

        private SyntaxToken Current => Peek(0);

        private SyntaxToken NextToken()
        {
            SyntaxToken current = Current;
            position++;
            return current;
        }

        private SyntaxToken MatchToken(SyntaxKind kind)
        {
            if (Current.Kind != kind)
            {
                diagnostics.ReportUnexpectedToken(Current.TextSpan, Current.Kind, kind);
                return new SyntaxToken(kind, position, null, null);
            }
            return NextToken();
        }

        public SyntaxTree Parse()
        {
            ExpressionSyntax expression = ParseExpression();
            SyntaxToken endOfFileToken = MatchToken(SyntaxKind.EndOfFileToken);
            return new SyntaxTree(diagnostics, expression, endOfFileToken);
        }
        
        private ExpressionSyntax ParseExpression(int parentPrecedence = 0)
        {
            ExpressionSyntax left;
            int unaryOperatorPrecedence = Current.Kind.GetUnaryOperatorPrecedence();

            if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence)
            {
                SyntaxToken operatorToken = NextToken();
                ExpressionSyntax operand = ParseExpression(unaryOperatorPrecedence);
                left = new UnaryExpressionSyntax(operatorToken, operand);
            }
            else
            {
                left = ParsePrimaryExpression();
            }

            while (true)
            {
                int precedence = Current.Kind.GetBinaryOperatorPrecedence();
                if (precedence == 0 || precedence <= parentPrecedence) break;

                SyntaxToken operatorToken = NextToken();
                ExpressionSyntax right = ParseExpression(precedence);
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }
        
        private ExpressionSyntax ParsePrimaryExpression()
        {
            switch (Current.Kind)
            {
                case SyntaxKind.OpenParenthesisToken:
                    {
                        SyntaxToken left = NextToken();
                        ExpressionSyntax expression = ParseExpression();
                        SyntaxToken right = MatchToken(SyntaxKind.CloseParenthesisToken);
                        return new ParenthesizedExpressionSyntax(left, expression, right);
                    }

                case SyntaxKind.TrueKeyword:
                case SyntaxKind.FalseKeyword:
                    {
                        SyntaxToken keywordToken = NextToken();
                        bool value = keywordToken.Kind == SyntaxKind.TrueKeyword;
                        return new LiteralExpressionSyntax(keywordToken, value);
                    }
                default:
                    {
                        SyntaxToken numberToken = MatchToken(SyntaxKind.NumberToken);
                        return new LiteralExpressionSyntax(numberToken);
                    }
            }
        }
    }
}
