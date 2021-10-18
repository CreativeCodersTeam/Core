using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

#nullable enable
namespace CreativeCoders.Core.Comparing
{
    public class ComparableObject<T> : IEquatable<T>, IComparable<T>
        where T : ComparableObject<T>
    {
        private static IEqualityComparer<T> EqualityComparer = EqualityComparer<T>.Default;

        private static IComparer<T> Comparer =
            new FuncComparer<T, string?>(x => x?.ToString(), SortOrder.Ascending);

        private static Func<T, int> GetHashCodeFunc = RuntimeHelpers.GetHashCode;

        protected static void InitComparableObject<TProperty>(Func<T, TProperty> getCompareProperty)
        {
            EqualityComparer = new FuncEqualityComparer<T, TProperty>(getCompareProperty);

            Comparer = new FuncComparer<T, TProperty>(getCompareProperty, SortOrder.Ascending);

            GetHashCodeFunc = x => EqualityComparer.GetHashCode(x);
        }

        public bool Equals(T? other)
        {
            return EqualityComparer.Equals((T)this, other);
        }

        public int CompareTo(T? other)
        {
            return Comparer.Compare((T)this, other);
        }

        public override bool Equals(object? obj) => Equals(obj as T);

        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return GetHashCodeFunc((T)this);
        }
    }
}
