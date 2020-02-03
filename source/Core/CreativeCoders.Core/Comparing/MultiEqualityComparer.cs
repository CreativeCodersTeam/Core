using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Comparing
{
    [PublicAPI]
    public class MultiEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly IEqualityComparer<T>[] _comparerList;

        public MultiEqualityComparer(params IEqualityComparer<T>[] comparerList)
        {
            Ensure.IsNotNullOrEmpty(comparerList, nameof(comparerList));

            _comparerList = comparerList;
        }

        public bool Equals(T x, T y)
        {
            return _comparerList.All(comparer => comparer.Equals(x, y));
        }

        public int GetHashCode(T obj)
        {
            if (_comparerList.Length == 1)
            {
                return _comparerList.First().GetHashCode(obj);
            }

            var hashCodes = _comparerList.Select(comparer => comparer.GetHashCode(obj));
            return string.Concat(hashCodes).GetHashCode();
        }
    }
}