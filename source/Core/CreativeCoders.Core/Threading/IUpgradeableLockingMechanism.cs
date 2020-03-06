namespace CreativeCoders.Core.Threading
{
    public interface IUpgradeableLockingMechanism : ILockingMechanism
    {
        void UpgradeableRead(UpgradeableReadAction action);

        T UpgradeableRead<T>(UpgradeableReadFunc<T> function);
    }
}