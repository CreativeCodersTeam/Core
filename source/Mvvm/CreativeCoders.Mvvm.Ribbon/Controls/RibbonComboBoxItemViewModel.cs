using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Ribbon.Controls;

[PublicAPI]
public class RibbonComboBoxItemViewModel : RibbonControlViewModel
{
    private string _icon;

    public string Icon
    {
        get => _icon;
        set => Set(ref _icon, value);
    }
}
