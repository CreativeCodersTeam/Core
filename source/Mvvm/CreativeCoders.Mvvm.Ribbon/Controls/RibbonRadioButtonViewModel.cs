using CreativeCoders.Core.ObjectLinking;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Ribbon.Controls;

[PublicAPI]
public class RibbonRadioButtonViewModel : RibbonCommandControlViewModel
{
    private bool? _isChecked;

    private string _smallIcon;

    public RibbonRadioButtonViewModel() {}

    public RibbonRadioButtonViewModel(ActionViewModel action) : base(action) {}

    [PropertyLink(typeof(ActionViewModel), nameof(ActionViewModel.IsChecked), Direction = LinkDirection.TwoWay, InitWithTargetValue = true)]
    public bool? IsChecked
    {
        get => _isChecked;
        set => Set(ref _isChecked, value);
    }

    [PropertyLink(typeof(ActionViewModel), nameof(ActionViewModel.SmallIcon), Direction = LinkDirection.TwoWay, InitWithTargetValue = true)]
    public string SmallIcon
    {
        get => _smallIcon;
        set => Set(ref _smallIcon, value);
    }        
}