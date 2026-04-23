using System;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Enums;

/// <summary>
/// Defines a contract for converting between enum values and their string representations.
/// </summary>
[PublicAPI]
public interface IEnumToStringConverter
{
    /// <summary>
    /// Converts the specified enum value to its string representation.
    /// </summary>
    /// <param name="enumValue">The enum value to convert.</param>
    /// <returns>The string representation of the enum value.</returns>
    string Convert(Enum enumValue);

    /// <summary>
    /// Converts the specified string to the corresponding enum value of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The enum type to convert to.</typeparam>
    /// <param name="text">The string to convert.</param>
    /// <returns>The enum value matching <paramref name="text"/>, or the default value if no match is found.</returns>
    T Convert<T>(string text)
        where T : Enum;
}
