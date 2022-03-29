using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Ribbon;

[PublicAPI]
public class RibbonViewModel : ViewModelBase
{
    public RibbonViewModel()
    {
        Tabs = new RibbonItemCollection<RibbonTabViewModel>();
    }

    public RibbonItemCollection<RibbonTabViewModel> Tabs { get; }
}