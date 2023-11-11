using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Ribbon.Controls;

[PublicAPI]
public class RibbonButtonPanelViewModel : RibbonControlViewModel
{
    private bool _separatorIsVisible;

    public RibbonItemCollection<RibbonControlViewModel> Items { get; } = new();

    public bool SeparatorIsVisible
    {
        get => _separatorIsVisible;
        set => Set(ref _separatorIsVisible, value);
    }
}
