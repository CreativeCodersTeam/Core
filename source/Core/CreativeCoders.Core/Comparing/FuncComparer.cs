using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CreativeCoders.Core.Comparing
{
    public class FuncComparer<T, TKey> : IComparer<T>
    {
        private readonly Func<T, TKey> _keySelector;

        private readonly SortOrder _sortOrder;

        public FuncComparer(Func<T, TKey> keySelector, SortOrder sortOrder)
        {
            Ensure.IsNotNull(keySelector, nameof(keySelector));

            _keySelector = keySelector;
            _sortOrder = sortOrder;
        }

        [SuppressMessage("ReSharper", "CompareNonConstrainedGenericWithNull")]
        public int Compare(T x, T y)
        {
            if (ReferenceEquals(x, y))
            {
                return 0;
            }

            if (x == null)
            {
                return GetCompareResult(-1);
            }

            if (y == null)
            {
                return GetCompareResult(1);
            }

            var xValue = _keySelector(x);
            var yValue = _keySelector(y);

            return GetCompareResult(Comparer<TKey>.Default.Compare(xValue, yValue));
        }

        private int GetCompareResult(int compareResult)
        {
            return _sortOrder == SortOrder.Ascending ? compareResult : compareResult * -1;
        }
    }
}