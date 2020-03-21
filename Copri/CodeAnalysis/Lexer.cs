﻿using System.Collections.Generic;

namespace Copri.CodeAnalysis
{
    internal class Lexer
    {
        private readonly string text;
        private int position;
        private readonly IList<string> diagnostics = new List<string>();

        public Lexer(string text)
        {
            this.text = text;
        }

        public IEnumerable<string> Diagnostics => diagnostics;
        private char Current
        {
            get
            {
                if (position >= text.Length)
                {
                    return '\0';
                }
                return text[position];
            }
        }

        private void Next() => position++;

        public SyntaxToken NextToken()
        {
            if (position >= text.Length) return new SyntaxToken(SyntaxKind.EndOfFileToken, position, "\0", null);

            if (char.IsDigit(Current))
            {
                int start = position;

                while (char.IsDigit(Current))
                {
                    Next();
                }
                int length = position - start;
                string tokenText = text.Substring(start, length);
                if (!int.TryParse(tokenText, out int value))
                {
                    diagnostics.Add($"The number '{text}' is not a valid int.");
                }
                return new SyntaxToken(SyntaxKind.NumberToken, start, tokenText, value);
            }

            if (char.IsWhiteSpace(Current))
            {
                int start = position;

                while (char.IsWhiteSpace(Current))
                {
                    Next();
                }
                int length = position - start;
                string tokenText = text.Substring(start, length);
                return new SyntaxToken(SyntaxKind.WhiteSpaceToken, start, tokenText, null);
            }

            if (Current == '+') return new SyntaxToken(SyntaxKind.PlusToken, position++, "+", null);
            else if (Current == '-') return new SyntaxToken(SyntaxKind.MinusToken, position++, "-", null);
            else if (Current == '*') return new SyntaxToken(SyntaxKind.StarToken, position++, "*", null);
            else if (Current == '/') return new SyntaxToken(SyntaxKind.SlashToken, position++, "/", null);
            else if (Current == '(') return new SyntaxToken(SyntaxKind.OpenParenthesisToken, position++, "(", null);
            else if (Current == ')') return new SyntaxToken(SyntaxKind.CloseParenthesisToken, position++, ")", null);
            diagnostics.Add($"ERROR: bad character in input: '{Current}'.");
            return new SyntaxToken(SyntaxKind.BadToken, position++, text.Substring(position - 1, 1), null);
        }
    }
}