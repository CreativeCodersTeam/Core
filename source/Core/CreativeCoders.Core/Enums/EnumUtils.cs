using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CreativeCoders.Core.Enums;

/// <summary>
/// Provides static utility methods for working with enum types and their reflection metadata.
/// </summary>
public static class EnumUtils
{
    // ReSharper disable once ReturnTypeCanBeEnumerable.Global
    /// <summary>
    /// Gets the <see cref="FieldInfo"/> for each value defined in the enum type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <returns>An array of <see cref="FieldInfo"/> instances for the enum values.</returns>
    public static FieldInfo[] GetValuesFieldInfos<T>()
        where T : Enum
    {
        var enumType = typeof(T);
        var enumValues = Enum.GetValues(enumType);

        return enumValues
            .Cast<Enum>()
            .Select(enumValue => enumType.GetField(enumValue.ToString()))
            .Where(fieldInfo => fieldInfo != null)
            .ToArray();
    }

    /// <summary>
    /// Gets all values defined in the enum type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <returns>An array of <see cref="Enum"/> values.</returns>
    public static Enum[] GetEnumValues<T>()
        where T : Enum
    {
        var values = Enum.GetValues(typeof(T));
        return values.Cast<Enum>().ToArray();
    }

    /// <summary>
    /// Gets the enum value of type <typeparamref name="T"/> that corresponds to the specified integer value.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="enumIntValue">The integer value to match.</param>
    /// <returns>
    /// The matching enum value, or the first defined value if no match is found.
    /// </returns>
    public static T GetEnum<T>(int enumIntValue)
        where T : Enum
    {
        var enumValues = GetEnumValues<T>();

        foreach (var enumValue in enumValues.Where(enumValue => Convert.ToInt32(enumValue) == enumIntValue))
        {
            return (T) enumValue;
        }

        return (T) enumValues.First();
    }

    /// <summary>
    /// Gets the <see cref="FieldInfo"/> for the specified enum value.
    /// </summary>
    /// <param name="enumValue">The enum value to look up.</param>
    /// <returns>The <see cref="FieldInfo"/> representing the enum field.</returns>
    public static FieldInfo GetFieldInfoForEnum(Enum enumValue)
    {
        Ensure.IsNotNull(enumValue);

        var enumType = enumValue.GetType();
        return enumType.GetField(enumValue.ToString());
    }

    /// <summary>
    /// Gets a dictionary mapping each enum value to its <see cref="FieldInfo"/> for the specified enum type.
    /// </summary>
    /// <param name="enumType">The enum type to inspect.</param>
    /// <returns>
    /// A dictionary mapping enum values to their <see cref="FieldInfo"/>, or an empty dictionary
    /// if <paramref name="enumType"/> is not an enum.
    /// </returns>
    public static IDictionary<Enum, FieldInfo> GetEnumFieldInfos(Type enumType)
    {
        if (!enumType.IsEnum)
        {
            return new Dictionary<Enum, FieldInfo>();
        }

        var enumValues = Enum.GetValues(enumType);

        var enumFieldInfos = new Dictionary<Enum, FieldInfo>();

        foreach (Enum enumValue in enumValues)
        {
            var fieldInfo = enumType.GetField(enumValue.ToString());

            if (fieldInfo != null)
            {
                enumFieldInfos.Add(enumValue, fieldInfo);
            }
        }

        return enumFieldInfos;
    }

    /// <summary>
    /// Gets a dictionary mapping each enum value to its <see cref="FieldInfo"/> for the enum type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <returns>A dictionary mapping enum values to their <see cref="FieldInfo"/>.</returns>
    public static IDictionary<Enum, FieldInfo> GetEnumFieldInfos<T>()
        where T : Enum
    {
        return GetEnumFieldInfos(typeof(T));
    }
}
