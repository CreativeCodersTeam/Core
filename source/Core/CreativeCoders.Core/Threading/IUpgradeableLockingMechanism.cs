using JetBrains.Annotations;

namespace CreativeCoders.Core.Threading;

[PublicAPI]
public interface IUpgradeableLockingMechanism : ILockingMechanism
{
    void UpgradeableRead(UpgradeableReadAction action);

    T UpgradeableRead<T>(UpgradeableReadFunc<T> function);
}
