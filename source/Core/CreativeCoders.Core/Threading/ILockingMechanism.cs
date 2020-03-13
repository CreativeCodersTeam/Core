using System;

namespace CreativeCoders.Core.Threading
{
    public interface ILockingMechanism
    {
        void Read(Action action);

        T Read<T>(Func<T> function);

        void Write(Action action);

        T Write<T>(Func<T> function);
    }
}
