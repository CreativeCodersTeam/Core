using JetBrains.Annotations;

namespace CreativeCoders.Core
{
    /// <summary>   Representing a text span. </summary>
    [PublicAPI]
    public class TextSpan
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes a new instance of the CreativeCoders.Core.TextSpan class. </summary>
        ///
        /// <param name="start">    The start of the text span. Must not be less 0. </param>
        /// <param name="length">   The length of the text span. Must not be less 0. </param>
        ///-------------------------------------------------------------------------------------------------
        public TextSpan(int start, int length)
        {
            Ensure.That(start >= 0, nameof(start), "Start must not be less 0");
            Ensure.That(length >= 0, nameof(length), "Length must not be less 0");

            Start = start;
            Length = length;
            End = start + length;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the start position of the text span. </summary>
        ///
        /// <value> The start position of the text span. </value>
        ///-------------------------------------------------------------------------------------------------
        public int Start { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the length of the text span. </summary>
        ///
        /// <value> The length of the text span. </value>
        ///-------------------------------------------------------------------------------------------------
        public int Length { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the end position of the text span. </summary>
        ///
        /// <value> The end position of the text span. </value>
        ///-------------------------------------------------------------------------------------------------
        public int End { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value indicating whether this text span is empty. </summary>
        ///
        /// <value> True if this text span is empty, false if not. </value>
        ///-------------------------------------------------------------------------------------------------
        public bool IsEmpty => Length == 0;
    }
}