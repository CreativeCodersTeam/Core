using CreativeCoders.Core.ObjectLinking;
using CreativeCoders.Mvvm.Wpf;
using CreativeCoders.Mvvm.Ribbon.FluentRibbon.LinkConverters;
using Fluent;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Ribbon.FluentRibbon;

[PublicAPI]
public class RibbonTabViewModel : ObjectLinkViewModelBase
{
    private string _text;

    private bool _isVisible;

    private bool _isSelected;

    public RibbonTabViewModel()
    {
        IsVisible = true;
    }

    [PropertyLink(typeof(RibbonTabItem), nameof(RibbonTabItem.IsSelected),
        Direction = LinkDirection.OneWayToTarget)]
    public bool IsSelected
    {
        get => _isSelected;
        set => Set(ref _isSelected, value);
    }

    [PropertyLink(typeof(RibbonTabItem), nameof(RibbonTabItem.Header),
        Direction = LinkDirection.OneWayToTarget)]
    public string Text
    {
        get => _text;
        set => Set(ref _text, value);
    }

    [PropertyLink(typeof(RibbonTabItem), nameof(RibbonTabItem.Visibility),
        Converter = typeof(BooleanToVisibilityPropertyConverter), Direction = LinkDirection.OneWayToTarget)]
    public bool IsVisible
    {
        get => _isVisible;
        set => Set(ref _isVisible, value);
    }
}
