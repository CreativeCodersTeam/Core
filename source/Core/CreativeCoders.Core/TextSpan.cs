using JetBrains.Annotations;

namespace CreativeCoders.Core
{
    [PublicAPI]
    public class TextSpan
    {
        public TextSpan(int start, int length)
        {
            Ensure.That(start >= 0, nameof(start), "Start must not be less 0");
            Ensure.That(length >= 0, nameof(length), "Length must not be less 0");

            Start = start;
            Length = length;
            End = start + length;
        }

        public int Start { get; }

        public int Length { get; }

        public int End { get; }

        public bool IsEmpty => Length == 0;
    }
}