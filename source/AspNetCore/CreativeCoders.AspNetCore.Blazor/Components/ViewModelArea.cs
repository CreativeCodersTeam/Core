using System;
using System.Collections.Generic;
using System.ComponentModel;
using CreativeCoders.AspNetCore.Blazor.Components.Base;
using CreativeCoders.Core;
using CreativeCoders.Core.Collections;
using CreativeCoders.Core.Threading;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace CreativeCoders.AspNetCore.Blazor.Components;

[PublicAPI]
public class ViewModelArea<TViewModel> : ContainerControlBase, IDisposable
    where TViewModel : class, INotifyPropertyChanged
{
    private IList<string> _observeProperties = new List<string>();

    private IList<string> _excludeProperties = new List<string>();

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        base.BuildRenderTree(builder);

        builder.AddContent(0, ChildContent);
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (ViewModel == null)
        {
            return;
        }

        ViewModel.PropertyChanged += ViewModelOnPropertyChanged;
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        _observeProperties = new List<string>(
            ObserveProperties?.Split(new[] {' ', ',', ';'}, StringSplitOptions.RemoveEmptyEntries)
                .WhereNotNull() ?? Array.Empty<string>());

        _excludeProperties = new List<string>(
            ExcludeProperties?.Split(new[] {' ', ',', ';'}, StringSplitOptions.RemoveEmptyEntries)
                .WhereNotNull() ?? Array.Empty<string>());
    }

    private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (_observeProperties.Count > 0 && !_observeProperties.Contains(e.PropertyName))
        {
            return;
        }

        if (_excludeProperties.Count > 0 && _excludeProperties.Contains(e.PropertyName))
        {
            return;
        }

        InvokeAsync(StateHasChanged).FireAndForgetAsync(_ => { });
    }

    public void Dispose()
    {
        if (ViewModel == null)
        {
            return;
        }

        ViewModel.PropertyChanged -= ViewModelOnPropertyChanged;
    }

    [Parameter] public TViewModel ViewModel { get; set; }

    [Parameter] public string ObserveProperties { get; set; }

    [Parameter] public string ExcludeProperties { get; set; }
}
