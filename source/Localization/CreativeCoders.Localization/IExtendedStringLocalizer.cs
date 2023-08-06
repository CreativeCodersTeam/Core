using Microsoft.Extensions.Localization;

namespace CreativeCoders.Localization;

///<inheritdoc cref="IStringLocalizer{T}"/>
public interface IExtendedStringLocalizer<out T> : IStringLocalizer<T> { }
