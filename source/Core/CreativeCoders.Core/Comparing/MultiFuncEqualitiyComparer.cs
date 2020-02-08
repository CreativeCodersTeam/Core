using System;
using System.Collections.Generic;
using System.Linq;

namespace CreativeCoders.Core.Comparing
{
    public class MultiFuncEqualityComparer<T, TKey> : MultiEqualityComparer<T>
    {
        public MultiFuncEqualityComparer(params Func<T, TKey>[] keySelectorFunctions)
            : base(keySelectorFunctions.Select(func => new FuncEqualityComparer<T, TKey>(func) as IEqualityComparer<T>).ToArray())
        {
        }
    }

    public class MultiFuncEqualityComparer<T, TKey1, TKey2> : MultiEqualityComparer<T>
    {
        public MultiFuncEqualityComparer(Func<T, TKey1> keySelectorFunc1, Func<T, TKey2> keySelectorFunc2) :
            base(new FuncEqualityComparer<T, TKey1>(keySelectorFunc1),
                new FuncEqualityComparer<T, TKey2>(keySelectorFunc2))
        {
        }
    }

    public class MultiFuncEqualityComparer<T, TKey1, TKey2, TKey3> : MultiEqualityComparer<T>
    {
        public MultiFuncEqualityComparer(Func<T, TKey1> keySelectorFunc1, Func<T, TKey2> keySelectorFunc2,
            Func<T, TKey3> keySelectorFunc3) :
            base(new FuncEqualityComparer<T, TKey1>(keySelectorFunc1),
                new FuncEqualityComparer<T, TKey2>(keySelectorFunc2),
                new FuncEqualityComparer<T, TKey3>(keySelectorFunc3))
        {
        }
    }

    public class MultiFuncEqualityComparer<T, TKey1, TKey2, TKey3, TKey4> : MultiEqualityComparer<T>
    {
        public MultiFuncEqualityComparer(Func<T, TKey1> keySelectorFunc1, Func<T, TKey2> keySelectorFunc2,
            Func<T, TKey3> keySelectorFunc3, Func<T, TKey4> keySelectorFunc4) :
            base(new FuncEqualityComparer<T, TKey1>(keySelectorFunc1),
                new FuncEqualityComparer<T, TKey2>(keySelectorFunc2),
                new FuncEqualityComparer<T, TKey3>(keySelectorFunc3),
                new FuncEqualityComparer<T, TKey4>(keySelectorFunc4))
        {
        }
    }

    public class MultiFuncEqualityComparer<T, TKey1, TKey2, TKey3, TKey4, TKey5> : MultiEqualityComparer<T>
    {
        public MultiFuncEqualityComparer(Func<T, TKey1> keySelectorFunc1, Func<T, TKey2> keySelectorFunc2,
            Func<T, TKey3> keySelectorFunc3, Func<T, TKey4> keySelectorFunc4, Func<T, TKey5> keySelectorFunc5) :
            base(new FuncEqualityComparer<T, TKey1>(keySelectorFunc1),
                new FuncEqualityComparer<T, TKey2>(keySelectorFunc2),
                new FuncEqualityComparer<T, TKey3>(keySelectorFunc3),
                new FuncEqualityComparer<T, TKey4>(keySelectorFunc4),
                new FuncEqualityComparer<T, TKey5>(keySelectorFunc5))
        {
        }
    }
}