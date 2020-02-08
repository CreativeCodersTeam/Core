using CreativeCoders.Core.ObjectLinking;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Ribbon.Controls
{
    [PublicAPI]
    public class RibbonMenuItemViewModel : RibbonCommandControlViewModel
    {
        private string _icon;

        private bool _isCheckable;

        private bool? _isChecked;

        public RibbonMenuItemViewModel() {}

        public RibbonMenuItemViewModel(ActionViewModel action) : base(action) {}

        [PropertyLink(typeof(ActionViewModel), nameof(ActionViewModel.SmallIcon), Direction = LinkDirection.TwoWay, InitWithTargetValue = true)]
        public string Icon
        {
            get => _icon;
            set => Set(ref _icon, value);
        }

        public bool IsCheckable
        {
            get => _isCheckable;
            set => Set(ref _isCheckable, value);
        }

        [PropertyLink(typeof(ActionViewModel), nameof(ActionViewModel.IsChecked), Direction = LinkDirection.TwoWay, InitWithTargetValue = true)]
        public bool? IsChecked
        {
            get => _isChecked;
            set => Set(ref _isChecked, value);
        }
    }
}
