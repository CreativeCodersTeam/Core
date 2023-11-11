using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Ribbon.Controls;

[PublicAPI]
public class RibbonDropButtonViewModel : RibbonBaseButtonViewModel
{
    public RibbonItemCollection<RibbonControlViewModel> Items { get; } = new();
}
