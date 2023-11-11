using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Ribbon.Controls;

[PublicAPI]
public class RibbonListBoxViewModel : RibbonControlViewModel
{
    private RibbonControlViewModel _selectedItem;

    public RibbonItemCollection<RibbonControlViewModel> Items { get; } = new();

    public RibbonControlViewModel SelectedItem
    {
        get => _selectedItem;
        set => Set(ref _selectedItem, value);
    }
}
