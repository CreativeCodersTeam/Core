using CreativeCoders.Core.ObjectLinking;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Ribbon.Controls
{
    [PublicAPI]
    public abstract class RibbonBaseButtonViewModel : RibbonCommandControlViewModel
    {
        private RibbonButtonSize _size;

        private string _largeIcon;

        private string _smallIcon;

        protected RibbonBaseButtonViewModel() {}

        protected RibbonBaseButtonViewModel(ActionViewModel action) : base(action) {}

        public RibbonButtonSize Size
        {
            get => _size;
            set => Set(ref _size, value);
        }

        [PropertyLink(typeof(ActionViewModel), nameof(ActionViewModel.LargeIcon), Direction = LinkDirection.TwoWay, InitWithTargetValue = true)]
        public string LargeIcon
        {
            get => _largeIcon;
            set => Set(ref _largeIcon, value);
        }

        [PropertyLink(typeof(ActionViewModel), nameof(ActionViewModel.SmallIcon), Direction = LinkDirection.TwoWay, InitWithTargetValue = true)]
        public string SmallIcon
        {
            get => _smallIcon;
            set => Set(ref _smallIcon, value);
        }        
    }
}
