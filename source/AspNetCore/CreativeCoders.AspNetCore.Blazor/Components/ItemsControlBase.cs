using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using CreativeCoders.AspNetCore.Blazor.Components.Base;
using CreativeCoders.Core.Collections;
using Microsoft.AspNetCore.Components;

namespace CreativeCoders.AspNetCore.Blazor.Components
{
    public class ItemsControlBase<TItem> : ControlBase, IDisposable
    {
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (ItemsSource == null)
            {
                return;
            }

            CopyFromSourceToItems();

            ItemsSource.CollectionChanged += ItemsSourceOnCollectionChanged;
        }

        private void ItemsSourceOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CopyFromSourceToItems();

            InvokeAsync(StateHasChanged);
        }

        private void CopyFromSourceToItems()
        {
            Items = new List<TItem>((IEnumerable<TItem>) ItemsSource ?? new TItem[0]);
        }

        [Parameter]
        public IReadOnlyCollection<TItem> Items { get; set; }

        [Parameter]
        public RenderFragment<TItem> ItemTemplate { get; set; }

        [Parameter]
        public ExtendedObservableCollection<TItem> ItemsSource { get; set; }

        public void Dispose()
        {
            if (ItemsSource != null)
            {
                ItemsSource.CollectionChanged -= ItemsSourceOnCollectionChanged;
            }
        }
    }
}