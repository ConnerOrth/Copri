using System.Collections.Generic;

namespace Copri.CodeAnalysis.Syntax
{
    internal sealed class Lexer
    {
        private readonly string text;
        private int position;
        private readonly DiagnosticBag diagnostics = new DiagnosticBag();

        public Lexer(string text)
        {
            this.text = text;
        }

        public DiagnosticBag Diagnostics => diagnostics;

        private char Current => Peek(0);
        private char Lookahead => Peek(1);

        private char Peek(int offset)
        {
            int index = position + offset;
            if (index >= text.Length)
            {
                return '\0';
            }
            return text[index];
        }

        private void Next() => position++;

        public SyntaxToken Lex()
        {
            if (position >= text.Length) return new SyntaxToken(SyntaxKind.EndOfFileToken, position, "\0", null);

            int start = position;
            if (char.IsDigit(Current))
            {
                while (char.IsDigit(Current))
                {
                    Next();
                }
                int length = position - start;
                string tokenText = text.Substring(start, length);
                if (!int.TryParse(tokenText, out int value))
                {
                    diagnostics.ReportInvalidNumber(new TextSpan(start, length), text, typeof(int));
                }
                return new SyntaxToken(SyntaxKind.NumberToken, start, tokenText, value);
            }

            if (char.IsWhiteSpace(Current))
            {
                while (char.IsWhiteSpace(Current))
                {
                    Next();
                }
                int length = position - start;
                string tokenText = text.Substring(start, length);
                return new SyntaxToken(SyntaxKind.WhiteSpaceToken, start, tokenText, null);
            }

            if (char.IsLetter(Current))
            {
                while (char.IsLetter(Current))
                {
                    Next();
                }
                int length = position - start;
                string tokenText = text.Substring(start, length);
                SyntaxKind kind = SyntaxFacts.GetKeywordKind(tokenText);
                return new SyntaxToken(kind, start, tokenText, null);
            }

            switch (Current)
            {
                case '+': return new SyntaxToken(SyntaxKind.PlusToken, position++, "+", null);
                case '-': return new SyntaxToken(SyntaxKind.MinusToken, position++, "-", null);
                case '*': return new SyntaxToken(SyntaxKind.StarToken, position++, "*", null);
                case '/': return new SyntaxToken(SyntaxKind.SlashToken, position++, "/", null);
                case '(': return new SyntaxToken(SyntaxKind.OpenParenthesisToken, position++, "(", null);
                case ')': return new SyntaxToken(SyntaxKind.CloseParenthesisToken, position++, ")", null);
                case '!' when Lookahead == '=':
                    {
                        position += 2;
                        return new SyntaxToken(SyntaxKind.BangEqualsToken, start, "!=", null);
                    }
                case '!': return new SyntaxToken(SyntaxKind.BangToken, position++, "!", null);
                case '&' when Lookahead == '&':
                    {
                        position += 2;
                        return new SyntaxToken(SyntaxKind.AmpersandAmpersandToken, start, "&&", null);
                    }
                case '|' when Lookahead == '|':
                    {
                        position += 2;
                        return new SyntaxToken(SyntaxKind.PipePipeToken, start, "||", null);
                    }
                case '=' when Lookahead == '=':
                    {
                        position += 2;
                        return new SyntaxToken(SyntaxKind.EqualsEqualsToken, start, "==", null);
                    }
                case '=':
                    {
                        return new SyntaxToken(SyntaxKind.EqualsToken, position++, "=", null);
                    }
            }
            diagnostics.ReportBadCharacter(position, Current);
            return new SyntaxToken(SyntaxKind.BadToken, position++, text.Substring(position - 1, 1), null);
        }
    }
}
