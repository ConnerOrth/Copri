namespace Copri.CodeAnalysis
{
    public sealed class Diagnostic
    {
        public TextSpan TextSpan { get; }
        public string Message { get; }

        public Diagnostic(TextSpan textSpan, string message)
            => (TextSpan, Message) = (textSpan, message);

        public override string ToString() => Message;
    }
}
