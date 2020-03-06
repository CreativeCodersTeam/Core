using System;
using CreativeCoders.Core.Weak;

namespace CreativeCoders.Core.Threading
{
    public class LockLockingMechanism : ILockingMechanism
    {
        private readonly object _lockObj;

        public LockLockingMechanism()
        {
            _lockObj = this;
        }

        public LockLockingMechanism(object lockObject)
        {
            Ensure.IsNotNull(lockObject, nameof(lockObject));

            _lockObj = lockObject;
        }

        public void Read(Action action)
        {
            lock (_lockObj)
            {
                action();
            }
        }

        public T Read<T>(Func<T> function)
        {
            lock (_lockObj)
            {
                return function();
            }
        }

        public void Write(Action action)
        {
            lock (_lockObj)
            {
                action();
            }
        }

        public T Write<T>(Func<T> function)
        {
            lock (_lockObj)
            {
                return function();
            }
        }
    }
}
