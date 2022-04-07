using CreativeCoders.Mvvm.Ribbon.Controls;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Ribbon;

[PublicAPI]
public class RibbonBarViewModel : RibbonItemViewModel
{
    public RibbonBarViewModel()
    {
        Items = new RibbonItemCollection<RibbonControlViewModel>();
    }

    public RibbonItemCollection<RibbonControlViewModel> Items { get; }
}
