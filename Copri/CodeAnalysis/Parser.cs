using System.Collections.Generic;

namespace Copri.CodeAnalysis
{
    internal sealed class Parser
    {
        private readonly IList<SyntaxToken> tokens = new List<SyntaxToken>();
        private readonly List<string> diagnostics = new List<string>();
        private int position;

        public Parser(string text)
        {
            Lexer lexer = new Lexer(text);
            SyntaxToken token;
            do
            {
                token = lexer.NextToken();
                if (token.Kind != SyntaxKind.WhiteSpaceToken &&
                    token.Kind != SyntaxKind.BadToken)
                {
                    tokens.Add(token);
                }
            } while (token.Kind != SyntaxKind.EndOfFileToken);

            diagnostics.AddRange(lexer.Diagnostics);
        }

        public IEnumerable<string> Diagnostics => diagnostics;

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
                diagnostics.Add($"ERROR: unexpected token <{Current.Kind}>, expected <{kind}>.");
                return new SyntaxToken(kind, position, null, null);
            }
            return NextToken();
        }

        private ExpressionSyntax ParseExpression() => ParseTerm();

        public SyntaxTree Parse()
        {
            ExpressionSyntax expression = ParseTerm();
            SyntaxToken endOfFileToken = MatchToken(SyntaxKind.EndOfFileToken);
            return new SyntaxTree(diagnostics, expression, endOfFileToken);
        }

        public ExpressionSyntax ParseTerm()
        {
            ExpressionSyntax left = ParseFactor();

            while (Current.Kind == SyntaxKind.PlusToken ||
                Current.Kind == SyntaxKind.MinusToken)
            {
                SyntaxToken operatorToken = NextToken();

                ExpressionSyntax right = ParseFactor();
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }

        public ExpressionSyntax ParseFactor()
        {
            ExpressionSyntax left = ParsePrimaryExpression();

            while (Current.Kind == SyntaxKind.StarToken ||
                Current.Kind == SyntaxKind.SlashToken)
            {
                SyntaxToken operatorToken = NextToken();

                ExpressionSyntax right = ParsePrimaryExpression();
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }

        private ExpressionSyntax ParsePrimaryExpression()
        {
            if (Current.Kind == SyntaxKind.OpenParenthesisToken)
            {
                SyntaxToken left = NextToken();
                ExpressionSyntax expression = ParseExpression();
                SyntaxToken right = MatchToken(SyntaxKind.CloseParenthesisToken);
                return new ParenthesizedExpressionSyntax(left, expression, right);
            }
            SyntaxToken numberToken = MatchToken(SyntaxKind.NumberToken);
            return new LiteralExpressionSyntax(numberToken);
        }
    }
}
