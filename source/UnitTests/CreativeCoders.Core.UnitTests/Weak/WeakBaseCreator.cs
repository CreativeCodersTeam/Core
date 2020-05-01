using System;
using CreativeCoders.Core.Weak;

namespace CreativeCoders.Core.UnitTests.Weak {
    public class WeakBaseCreator
    {
        public WeakBase<Action> CreateWeakBase()
        {
            var writer = new TestConsoleWriter();
            
            var weakAction = new WeakBase<Action>(this, () => writer.Write("Test"), KeepOwnerAliveMode.NotKeepAlive);
            return weakAction;
        }

        // ReSharper disable once MemberCanBeMadeStatic.Global
        public WeakBase<Action> CreateWeakBaseWithoutOwner()
        {
            var writer = new TestConsoleWriter();
            
            var weakAction = new WeakBase<Action>(null, () => writer.Write("Test"), KeepOwnerAliveMode.NotKeepAlive);
            return weakAction;
        }
    }
}