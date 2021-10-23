using System;
using System.Collections.Generic;
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

    public class ComparableObject<TObject, TInterface> : IEquatable<TInterface>, IComparable<TInterface>
        where TObject : ComparableObject<TObject, TInterface>, TInterface
        where TInterface : class
    {
        private static IEqualityComparer<TInterface> EqualityComparer = EqualityComparer<TInterface>.Default;

        private static IComparer<TInterface> Comparer =
            new FuncComparer<TInterface, string?>(x => x?.ToString(), SortOrder.Ascending);

        private static Func<TInterface, int> GetHashCodeFunc = RuntimeHelpers.GetHashCode;

        protected static void InitComparableObject<TProperty>(Func<TInterface, TProperty> getCompareProperty)
        {
            EqualityComparer = new FuncEqualityComparer<TInterface, TProperty>(getCompareProperty);

            Comparer = new FuncComparer<TInterface, TProperty>(getCompareProperty, SortOrder.Ascending);

            GetHashCodeFunc = x => EqualityComparer.GetHashCode(x);
        }

        public bool Equals(TInterface? other)
        {
            return EqualityComparer.Equals((TInterface)(object)this, other);
        }

        public int CompareTo(TInterface? other)
        {
            return Comparer.Compare((TInterface)(object)this, other);
        }

        public override bool Equals(object? obj) => Equals(obj as TInterface);

        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return GetHashCodeFunc((TInterface)(object)this);
        }
    }
}
