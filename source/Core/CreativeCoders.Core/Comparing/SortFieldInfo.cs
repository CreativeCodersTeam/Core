using System;

namespace CreativeCoders.Core.Comparing
{
    public class SortFieldInfo<T, TKey>
    {
        public SortFieldInfo(Func<T, TKey> keySelector, SortOrder sortOrder)
        {
            KeySelector = keySelector;
            SortOrder = sortOrder;
        }

        public Func<T, TKey> KeySelector { get; }

        public SortOrder SortOrder { get; }
    }
}