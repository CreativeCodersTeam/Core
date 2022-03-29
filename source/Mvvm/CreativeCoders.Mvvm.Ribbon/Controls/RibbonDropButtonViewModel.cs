using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Ribbon.Controls;

[PublicAPI]
public class RibbonDropButtonViewModel : RibbonBaseButtonViewModel
{
    public RibbonDropButtonViewModel()
    {
        Items = new RibbonItemCollection<RibbonControlViewModel>();
    }

    public RibbonItemCollection<RibbonControlViewModel> Items { get; }
}