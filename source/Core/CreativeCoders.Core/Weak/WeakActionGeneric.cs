using System;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Weak
{
    [PublicAPI]
    public class WeakAction<T> : WeakActionBase, IExecutable, IExecutable<T>
    {
        public WeakAction(Action<T> action) : this(action?.Target, action) {}

        public WeakAction(Action<T> action, KeepActionTargetAliveMode keepActionTargetAliveMode)
            : this(action?.Target, action, keepActionTargetAliveMode) { }

        public WeakAction(object target, Action<T> action)
            : base(target ?? action?.Target, action?.Method, action?.Target) {}

        public WeakAction(object target, Action<T> action, KeepActionTargetAliveMode keepActionTargetAliveMode)
            : base(target ?? action?.Target, action?.Method, action?.Target, keepActionTargetAliveMode) { }

        public void Execute()
        {
            Execute(default);
        }

        public void Execute(T parameter)
        {
            var action = CreateAction<Action<T>>();
            action?.Invoke(parameter);
        }
    }
}