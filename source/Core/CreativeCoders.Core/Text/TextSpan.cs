using JetBrains.Annotations;

namespace CreativeCoders.Core.Text;

/// <summary>
/// Represents an immutable span of text defined by a start position and length.
/// </summary>
[PublicAPI]
public class TextSpan
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TextSpan"/> class.
    /// </summary>
    /// <param name="start">The zero-based start position. Must not be negative.</param>
    /// <param name="length">The length of the span. Must not be negative.</param>
    public TextSpan(int start, int length)
    {
        Ensure.That(start >= 0, "Start must not be less 0");
        Ensure.That(length >= 0, "Length must not be less 0");

        Start = start;
        Length = length;
        End = start + length;
    }

    /// <summary>
    /// Gets the zero-based start position of the text span.
    /// </summary>
    public int Start { get; }

    /// <summary>
    /// Gets the length of the text span.
    /// </summary>
    public int Length { get; }

    /// <summary>
    /// Gets the exclusive end position of the text span, computed as <see cref="Start"/> + <see cref="Length"/>.
    /// </summary>
    public int End { get; }

    /// <summary>
    /// Gets a value indicating whether the text span is empty (length is zero).
    /// </summary>
    public bool IsEmpty => Length == 0;
}
