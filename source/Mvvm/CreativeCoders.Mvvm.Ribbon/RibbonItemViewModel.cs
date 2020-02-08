using CreativeCoders.Core.ObjectLinking;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Ribbon
{
    [PublicAPI]
    public abstract class RibbonItemViewModel : ViewModelBase
    {
        private string _text;

        private string _name;

        private bool _isVisible;

        protected RibbonItemViewModel()
        {
            IsVisible = true;
        }

        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        [PropertyLink(typeof(ActionViewModel), nameof(ActionViewModel.Caption), Direction = LinkDirection.TwoWay, InitWithTargetValue = true)]
        public string Text
        {
            get => _text;
            set => Set(ref _text, value);
        }

        [PropertyLink(typeof(ActionViewModel), nameof(ActionViewModel.IsVisible), Direction = LinkDirection.TwoWay, InitWithTargetValue = true)]
        public bool IsVisible
        {
            get => _isVisible;
            set => Set(ref _isVisible, value);
        }
    }
}
