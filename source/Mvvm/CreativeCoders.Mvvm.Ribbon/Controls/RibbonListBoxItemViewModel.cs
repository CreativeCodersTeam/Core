using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Ribbon.Controls;

[PublicAPI]
public class RibbonListBoxItemViewModel : RibbonControlViewModel
{
    private string _icon;

    private bool _isCheckable;

    private bool? _isChecked;

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

    public bool? IsChecked
    {
        get => _isChecked;
        set => Set(ref _isChecked, value);
    }
}
