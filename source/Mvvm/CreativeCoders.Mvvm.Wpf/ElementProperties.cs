using System.Windows;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Wpf;

[PublicAPI]
public static class ElementProperties
{
    public static readonly DependencyProperty ObjectLinkProperty = DependencyProperty.RegisterAttached(
        "ObjectLink", typeof(ObjectLinkViewModelBase), typeof(ElementProperties),
        new PropertyMetadata(default(ObjectLinkViewModelBase), ObjectLinkPropertyChangedCallback));

    private static void ObjectLinkPropertyChangedCallback(DependencyObject element,
        DependencyPropertyChangedEventArgs e)
    {
        var oldLinkObject = e.OldValue as ObjectLinkViewModelBase;
        oldLinkObject?.Disconnect();

        var newLinkObject = e.NewValue as ObjectLinkViewModelBase;

        newLinkObject?.ConnectTo(element);
    }

    public static void SetObjectLink(DependencyObject element, ObjectLinkViewModelBase value)
    {
        element.SetValue(ObjectLinkProperty, value);
    }

    public static ObjectLinkViewModelBase GetObjectLink(DependencyObject element)
    {
        return (ObjectLinkViewModelBase) element.GetValue(ObjectLinkProperty);
    }
}
