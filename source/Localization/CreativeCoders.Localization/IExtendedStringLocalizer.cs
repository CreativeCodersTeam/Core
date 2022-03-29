using Microsoft.Extensions.Localization;

#nullable enable
namespace CreativeCoders.Localization;

///<inheritdoc cref="IStringLocalizer{T}"/>
public interface IExtendedStringLocalizer<out T> : IStringLocalizer<T>
{
        
}