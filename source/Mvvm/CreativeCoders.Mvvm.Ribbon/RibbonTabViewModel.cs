using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Ribbon;

[PublicAPI]
public class RibbonTabViewModel : RibbonItemViewModel
{
    public RibbonItemCollection<RibbonBarViewModel> Bars { get; } = new();
}
