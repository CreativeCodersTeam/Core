using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Ribbon.Controls;

[PublicAPI]
public class RibbonListBoxViewModel : RibbonControlViewModel
{
    private RibbonControlViewModel _selectedItem;

    public RibbonListBoxViewModel()
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
