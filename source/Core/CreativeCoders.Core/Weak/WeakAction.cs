using System;
using CreativeCoders.Core.Executing;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Weak
{
    [PublicAPI]
    public class WeakAction : WeakBase<Action>, IExecutable
    {
        public WeakAction(Action action) : this(action?.Target, action) {}

        public WeakAction(Action action, KeepTargetAliveMode keepTargetAliveMode)
            : this(action?.Target, action, keepTargetAliveMode) { }

        public WeakAction(object target, Action action)
            : base(target ?? action?.Target, action, KeepTargetAliveMode.NotKeepAlive) {}

        public WeakAction(object target, Action action, KeepTargetAliveMode keepTargetAliveMode)
            : base(target ?? action?.Target, action, keepTargetAliveMode) { }

        public void Execute()
        {
            var action = GetData();
            action?.Invoke();
        }
    }
}