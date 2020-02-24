using System;
using CreativeCoders.Core.Executing;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Weak
{
    [PublicAPI]
    public class WeakFunc<T> : WeakBase<Func<T>>, IExecutableWithResult<T>
    {
        public WeakFunc(Func<T> function) : this(function?.Target, function) { }

        public WeakFunc(Func<T> function, KeepOwnerAliveMode keepOwnerAliveMode)
            : this(function?.Target, function, keepOwnerAliveMode) { }

        public WeakFunc(object target, Func<T> function)
            : this(target, function, KeepOwnerAliveMode.NotKeepAlive) { }

        public WeakFunc(object target, Func<T> function, KeepOwnerAliveMode keepOwnerAliveMode)
            : base(target ?? function?.Target, function, keepOwnerAliveMode) { }

        public T Execute()
        {
            var function = GetData();
            return function();
        }
    }

    [PublicAPI]
    public class WeakFunc<TParameter, TResult> : WeakBase<Func<TParameter, TResult>>,
        IExecutableWithResult<TParameter, TResult>
    {
        public WeakFunc(Func<TParameter, TResult> function) : this(function?.Target, function) { }

        public WeakFunc(Func<TParameter, TResult> function, KeepOwnerAliveMode keepOwnerAliveMode)
            : this(function?.Target, function, keepOwnerAliveMode) { }

        public WeakFunc(object target, Func<TParameter, TResult> function)
            : base(target ?? function?.Target, function, KeepOwnerAliveMode.NotKeepAlive) { }

        public WeakFunc(object target, Func<TParameter, TResult> function, KeepOwnerAliveMode keepOwnerAliveMode)
            : base(target ?? function?.Target, function, keepOwnerAliveMode) { }

        public TResult Execute(TParameter parameter)
        {
            var function = GetData();
            return function(parameter);
        }
    }
}