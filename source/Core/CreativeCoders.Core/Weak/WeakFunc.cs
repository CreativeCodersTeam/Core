using System;
using CreativeCoders.Core.Executing;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Weak
{
    [PublicAPI]
    public class WeakFunc<T> : WeakBase<Func<T>>, IExecutableWithResult<T>
    {
        public WeakFunc(Func<T> function) : this(function?.Target, function) {}

        public WeakFunc(Func<T> function, KeepTargetAliveMode keepTargetAliveMode)
            : this(function?.Target, function, keepTargetAliveMode) { }

        public WeakFunc(object target, Func<T> function)
            : base(target ?? function?.Target, function, KeepTargetAliveMode.NotKeepAlive) {}

        public WeakFunc(object target, Func<T> function, KeepTargetAliveMode keepTargetAliveMode)
            : base(target ?? function?.Target, function, keepTargetAliveMode) { }

        public T Execute()
        {
            var function = GetData();
            return function();
        }
    }
    
    [PublicAPI]
    public class WeakFunc<TParameter, TResult> : WeakBase<Func<TParameter, TResult>>, IExecutableWithResult<TParameter, TResult>
    {
        public WeakFunc(Func<TParameter, TResult> function) : this(function?.Target, function) {}

        public WeakFunc(Func<TParameter, TResult> function, KeepTargetAliveMode keepTargetAliveMode)
            : this(function?.Target, function, keepTargetAliveMode) { }

        public WeakFunc(object target, Func<TParameter, TResult> function)
            : base(target ?? function?.Target, function, KeepTargetAliveMode.NotKeepAlive) {}

        public WeakFunc(object target, Func<TParameter, TResult> function, KeepTargetAliveMode keepTargetAliveMode)
            : base(target ?? function?.Target, function, keepTargetAliveMode) { }

        public TResult Execute(TParameter parameter)
        {
            var function = GetData();
            return function(parameter);
        }
    }
}