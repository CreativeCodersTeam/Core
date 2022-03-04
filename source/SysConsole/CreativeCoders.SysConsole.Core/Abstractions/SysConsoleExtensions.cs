using System;
using System.Collections.Generic;
using System.Linq;
using CreativeCoders.Core.Collections;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Core.Abstractions
{
    [PublicAPI]
    public static class SysConsoleExtensions
    {
        public static TItem? SelectItem<TItem>(this ISysConsole sysConsole, IEnumerable<TItem> items)
        {
            return sysConsole.SelectItem(items, x => x?.ToString());
        }

        public static TItem? SelectItem<TItem>(this ISysConsole sysConsole, IEnumerable<TItem> items, Func<TItem, string?> getCaption)
        {
            return sysConsole.SelectItem(items, getCaption, default);
        }

        public static TItem? SelectItem<TItem>(this ISysConsole sysConsole, IEnumerable<TItem> items, TItem? defaultItem)
        {
            return sysConsole.SelectItem(items, x => x?.ToString(), defaultItem);
        }

        public static TItem? SelectItem<TItem>(this ISysConsole sysConsole, IEnumerable<TItem> items, Func<TItem, string?> getCaption, TItem? defaultItem)
        {
            var selectionList = items
                .SelectWithIndex()
                .Select(x => new { Number = x.Index + 1, Text = getCaption(x.Data), Item = x.Data })
                .ToArray();

            selectionList.ForEach(x => sysConsole.WriteLine($"[{x.Number}]: {x.Text}"));

            var selection = sysConsole.ReadLine();

            if (!int.TryParse(selection, out var selectionNumber))
            {
                throw new InvalidOperationException();
            }

            var selectedItem = selectionList.FirstOrDefault(x => x.Number == selectionNumber);

            return selectedItem is null
                ? defaultItem
                : selectedItem.Item;
        }

        public static string? ReadLine(this ISysConsole sysConsole, string text)
        {
            return sysConsole
                .Write(text)
                .ReadLine();
        }
    }
}
