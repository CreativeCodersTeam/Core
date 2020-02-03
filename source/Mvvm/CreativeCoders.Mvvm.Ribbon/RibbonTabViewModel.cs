using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Ribbon
{
    [PublicAPI]
    public class RibbonTabViewModel : RibbonItemViewModel
    {
        public RibbonTabViewModel()
        {
            Bars = new RibbonItemCollection<RibbonBarViewModel>();
        }

        public RibbonItemCollection<RibbonBarViewModel> Bars { get; }
    }
}
