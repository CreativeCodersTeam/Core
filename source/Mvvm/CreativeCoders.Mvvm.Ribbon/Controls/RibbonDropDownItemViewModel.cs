using CreativeCoders.Core.ObjectLinking;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Ribbon.Controls;

[PublicAPI]
public class RibbonDropDownItemViewModel : RibbonCommandControlViewModel
{
    private string _icon;

    public RibbonDropDownItemViewModel() {}

    public RibbonDropDownItemViewModel(ActionViewModel action) : base(action) {}

    [PropertyLink(typeof(ActionViewModel), nameof(ActionViewModel.SmallIcon), Direction = LinkDirection.TwoWay, InitWithTargetValue = true)]
    public string Icon
    {
        get => _icon;
        set => Set(ref _icon, value);
    }
}