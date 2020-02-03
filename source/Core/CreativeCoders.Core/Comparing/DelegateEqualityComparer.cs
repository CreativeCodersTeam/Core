using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Comparing
{
    [PublicAPI]
    public class DelegateEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _compareFunc;

        private readonly Func<T, int> _hashCodeFunc;

        public DelegateEqualityComparer(Func<T, T, bool> compareFunc) : this(compareFunc, null)
        {
        }

        public DelegateEqualityComparer(Func<T, T, bool> compareFunc, Func<T, int> hashCodeFunc)
        {
            Ensure.IsNotNull(compareFunc, nameof(compareFunc));
            
            _compareFunc = compareFunc;
            _hashCodeFunc = hashCodeFunc;
        }

        public bool Equals(T x, T y)
        {
            return _compareFunc(x, y);
        }

        public int GetHashCode(T obj)
        {
            return _hashCodeFunc?.Invoke(obj) ?? obj.GetHashCode();
        }
    }
}