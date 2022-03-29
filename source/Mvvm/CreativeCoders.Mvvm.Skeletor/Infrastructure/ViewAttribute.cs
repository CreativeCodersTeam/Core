using System;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Skeletor.Infrastructure;

[PublicAPI]
[AttributeUsage(AttributeTargets.Class)]
public class ViewAttribute : Attribute
{
    public ViewAttribute(Type viewModelType)
    {
        ViewModelType = viewModelType;
    }

    public Type ViewModelType { get; }
}
