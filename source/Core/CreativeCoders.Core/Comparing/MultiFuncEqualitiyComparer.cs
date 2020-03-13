using System;
using System.Collections.Generic;
using System.Linq;

namespace CreativeCoders.Core.Comparing
{
    public class MultiFuncEqualityComparer<T, TKey> : MultiEqualityComparer<T>
    {
        public MultiFuncEqualityComparer(params Func<T, TKey>[] keySelectors)
            : base(keySelectors.Select(func => new FuncEqualityComparer<T, TKey>(func) as IEqualityComparer<T>).ToArray())
        {
        }
    }

    public class MultiFuncEqualityComparer<T, TKey1, TKey2> : MultiEqualityComparer<T>
    {
        public MultiFuncEqualityComparer(Func<T, TKey1> keySelector1, Func<T, TKey2> keySelector2) :
            base(new FuncEqualityComparer<T, TKey1>(keySelector1),
                new FuncEqualityComparer<T, TKey2>(keySelector2))
        {
        }
    }

    public class MultiFuncEqualityComparer<T, TKey1, TKey2, TKey3> : MultiEqualityComparer<T>
    {
        public MultiFuncEqualityComparer(Func<T, TKey1> keySelector1, Func<T, TKey2> keySelector2,
            Func<T, TKey3> keySelector3) :
            base(new FuncEqualityComparer<T, TKey1>(keySelector1),
                new FuncEqualityComparer<T, TKey2>(keySelector2),
                new FuncEqualityComparer<T, TKey3>(keySelector3))
        {
        }
    }

    public class MultiFuncEqualityComparer<T, TKey1, TKey2, TKey3, TKey4> : MultiEqualityComparer<T>
    {
        public MultiFuncEqualityComparer(Func<T, TKey1> keySelector1, Func<T, TKey2> keySelector2,
            Func<T, TKey3> keySelector3, Func<T, TKey4> keySelector4) :
            base(new FuncEqualityComparer<T, TKey1>(keySelector1),
                new FuncEqualityComparer<T, TKey2>(keySelector2),
                new FuncEqualityComparer<T, TKey3>(keySelector3),
                new FuncEqualityComparer<T, TKey4>(keySelector4))
        {
        }
    }

    public class MultiFuncEqualityComparer<T, TKey1, TKey2, TKey3, TKey4, TKey5> : MultiEqualityComparer<T>
    {
        public MultiFuncEqualityComparer(Func<T, TKey1> keySelector1, Func<T, TKey2> keySelector2,
            Func<T, TKey3> keySelector3, Func<T, TKey4> keySelector4, Func<T, TKey5> keySelector5) :
            base(new FuncEqualityComparer<T, TKey1>(keySelector1),
                new FuncEqualityComparer<T, TKey2>(keySelector2),
                new FuncEqualityComparer<T, TKey3>(keySelector3),
                new FuncEqualityComparer<T, TKey4>(keySelector4),
                new FuncEqualityComparer<T, TKey5>(keySelector5))
        {
        }
    }
}