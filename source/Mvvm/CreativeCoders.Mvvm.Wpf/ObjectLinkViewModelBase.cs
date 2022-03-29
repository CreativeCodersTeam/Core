using System.Windows;
using CreativeCoders.Core.ObjectLinking;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Wpf;

[PublicAPI]
public class ObjectLinkViewModelBase : ViewModelBase
{
    private string _name;
        
    private ObjectLink _elementLink;
        
    public void ConnectTo(DependencyObject element)
    {
        if (element is FrameworkElement frameworkElement)
        {
            frameworkElement.Name = Name;
        }
        _elementLink = new ObjectLinkBuilder(this, element).Build();
    }

    public void Disconnect()
    {
        _elementLink?.Dispose();
        _elementLink = null;
    }
        
    public string Name
    {
        get => _name;
        set => Set(ref _name, value);
    }
}