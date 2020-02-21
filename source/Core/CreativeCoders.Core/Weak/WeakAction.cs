using System;
using CreativeCoders.Core.Executing;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Weak
{
    [PublicAPI]
    public class WeakAction : WeakBase<Action>, IExecutable
    {
        public WeakAction(Action action) : this(action?.Target, action) {}

        public WeakAction(Action action, KeepOwnerAliveMode keepOwnerAliveMode)
            : this(action?.Target, action, keepOwnerAliveMode) { }

        public WeakAction(object target, Action action)
            : base(target ?? action?.Target, action, KeepOwnerAliveMode.NotKeepAlive) {}

        public WeakAction(object target, Action action, KeepOwnerAliveMode keepOwnerAliveMode)
            : base(target ?? action?.Target, action, keepOwnerAliveMode) { }

        public void Execute()
        {
            var action = GetData();
            action?.Invoke();
        }
    }
}