using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Ribbon.Controls;

[PublicAPI]
public class RibbonButtonViewModel : RibbonBaseButtonViewModel
{
    private bool _isToggle;

    public RibbonButtonViewModel() { }

    public RibbonButtonViewModel(ActionViewModel action) : base(action) { }

    public bool IsToggle
    {
        get => _isToggle;
        set => Set(ref _isToggle, value);
    }
}
