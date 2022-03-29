using System;
using CreativeCoders.Core.Executing;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Weak;

[PublicAPI]
public class WeakAction : WeakBase<Action>, IExecutable
{
    public WeakAction(Action action) : this(action?.Target, action) {}

    public WeakAction(Action action, KeepOwnerAliveMode keepOwnerAliveMode)
        : this(action?.Target, action, keepOwnerAliveMode) { }

    public WeakAction(object target, Action action)
        : base(target ?? action?.Target, action, GetAliveMode(action)) {}

    public WeakAction(object target, Action action, KeepOwnerAliveMode keepOwnerAliveMode)
        : base(target ?? action?.Target, action, keepOwnerAliveMode) { }

    private static KeepOwnerAliveMode GetAliveMode(Action action)
    {
        return action?.Method.IsStatic == true
            ? KeepOwnerAliveMode.KeepAlive
            : KeepOwnerAliveMode.AutoGuess;
    }

    public void Execute()
    {
        var action = GetData();
        action?.Invoke();
    }
}