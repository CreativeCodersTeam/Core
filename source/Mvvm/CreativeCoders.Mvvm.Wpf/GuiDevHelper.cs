using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Wpf;

[ExcludeFromCodeCoverage]
[PublicAPI]
public static class GuiDevHelper
{
    private static readonly Lazy<bool> IsInDesignModeLazy =
        new(() => DesignerProperties.GetIsInDesignMode(new DependencyObject()));

    public static bool IsInDesignMode => IsInDesignModeLazy.Value;
}
