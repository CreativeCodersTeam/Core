using System;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Weak
{
    [PublicAPI]
    public class WeakAction : WeakActionBase, IExecutable
    {
        public WeakAction(Action action) : this(action?.Target, action) {}

        public WeakAction(Action action, KeepActionTargetAliveMode keepActionTargetAliveMode)
            : this(action?.Target, action, keepActionTargetAliveMode) { }

        public WeakAction(object target, Action action)
            : base(target ?? action?.Target, action?.Method, action?.Target) {}

        public WeakAction(object target, Action action, KeepActionTargetAliveMode keepActionTargetAliveMode)
            : base(target ?? action?.Target, action?.Method, action?.Target, keepActionTargetAliveMode) { }

        public void Execute()
        {
            var action = CreateAction<Action>();
            action?.Invoke();
        }
    }
}