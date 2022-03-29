using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace CreativeCoders.Mvvm.Wpf;

[ExcludeFromCodeCoverage]
public static class GuiDevHelper
{
    private static readonly Lazy<bool> IsInDesignModeLazy =
        new(() => DesignerProperties.GetIsInDesignMode(new DependencyObject()));

    public static bool IsInDesignMode => IsInDesignModeLazy.Value;
}