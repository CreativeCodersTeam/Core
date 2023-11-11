using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Ribbon;

[PublicAPI]
public class RibbonViewModel : ViewModelBase
{
    public RibbonItemCollection<RibbonTabViewModel> Tabs { get; } = new();
}
