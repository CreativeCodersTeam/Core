using System;
using CreativeCoders.Core.Executing;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Weak
{
    [PublicAPI]
    public class WeakAction<T> : WeakBase<Action<T>>, IExecutable, IExecutable<T>, IExecutableWithParameter
    {
        public WeakAction(Action<T> action) : this(action?.Target, action) {}

        public WeakAction(Action<T> action, KeepTargetAliveMode keepTargetAliveMode)
            : this(action?.Target, action, keepTargetAliveMode) { }

        public WeakAction(object target, Action<T> action)
            : base(target ?? action?.Target, action, KeepTargetAliveMode.NotKeepAlive) {}

        public WeakAction(object target, Action<T> action, KeepTargetAliveMode keepTargetAliveMode)
            : base(target ?? action?.Target, action, keepTargetAliveMode) { }

        public void Execute()
        {
            Execute(default);
        }

        public void Execute(T parameter)
        {
            var action = GetData();
            action?.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
            Execute((T) parameter);
        }
    }
}