using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CreativeCoders.Core.Comparing
{
    public class FuncEqualityComparer<T, TKey> : IEqualityComparer<T>
    {
        private readonly Func<T, TKey> _keySelectorFunc;

        public FuncEqualityComparer(Func<T, TKey> keySelectorFunc)
        {
            Ensure.IsNotNull(keySelectorFunc, nameof(keySelectorFunc));

            _keySelectorFunc = keySelectorFunc;
        }

        [SuppressMessage("ReSharper", "CompareNonConstrainedGenericWithNull")]
        public bool Equals(T x, T y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            var xValue = _keySelectorFunc(x);
            var yValue = _keySelectorFunc(y);

            return EqualityComparer<TKey>.Default.Equals(xValue, yValue);
        }

        public int GetHashCode(T obj)
        {
            return _keySelectorFunc(obj)?.GetHashCode() ?? 0;
        }
    }
}