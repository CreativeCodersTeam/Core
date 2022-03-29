using System;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Enums;

[PublicAPI]
public interface IEnumToStringConverter
{
    string Convert(Enum enumValue);

    T Convert<T>(string text)
        where T : Enum;
}