using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Ribbon.Controls
{
    [PublicAPI]
    public class RibbonButtonPanelViewModel : RibbonControlViewModel
    {
        private bool _separatorIsVisible;

        public RibbonButtonPanelViewModel()
        {
            Items = new RibbonItemCollection<RibbonControlViewModel>();
        }

        public RibbonItemCollection<RibbonControlViewModel> Items { get; }

        public bool SeparatorIsVisible
        {
            get => _separatorIsVisible;
            set => Set(ref _separatorIsVisible, value);
        }
    }
}
