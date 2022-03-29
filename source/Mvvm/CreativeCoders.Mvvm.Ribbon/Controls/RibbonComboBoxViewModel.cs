using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Ribbon.Controls;

[PublicAPI]
public class RibbonComboBoxViewModel : RibbonControlViewModel
{
    private RibbonControlViewModel _selectedItem;

    public RibbonComboBoxViewModel()
    {
        Items = new RibbonItemCollection<RibbonControlViewModel>();
    }

    public RibbonItemCollection<RibbonControlViewModel> Items { get; }

    public RibbonControlViewModel SelectedItem
    {
        get => _selectedItem;
        set => Set(ref _selectedItem, value);
    }
}
