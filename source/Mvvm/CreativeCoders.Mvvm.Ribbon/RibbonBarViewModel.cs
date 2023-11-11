using CreativeCoders.Mvvm.Ribbon.Controls;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Ribbon;

[PublicAPI]
public class RibbonBarViewModel : RibbonItemViewModel
{
    public RibbonItemCollection<RibbonControlViewModel> Items { get; } = new();
}
