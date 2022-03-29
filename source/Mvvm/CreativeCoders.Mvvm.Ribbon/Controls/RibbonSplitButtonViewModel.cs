using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Ribbon.Controls;

[PublicAPI]
public class RibbonSplitButtonViewModel : RibbonBaseButtonViewModel
{
    public RibbonSplitButtonViewModel()
    {
        Items = new RibbonItemCollection<RibbonControlViewModel>();
    }

    public RibbonSplitButtonViewModel(ActionViewModel action) : base(action)
    {
        Items = new RibbonItemCollection<RibbonControlViewModel>();
    }

    public RibbonItemCollection<RibbonControlViewModel> Items { get; }
}