using System;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Threading
{
    [PublicAPI]
    public class SynchronizedValue<T>
    {
        private readonly ILockingMechanism _lockingMechanism;

        private T _value;

        public SynchronizedValue() : this(default(T)) { }

        public SynchronizedValue(ILockingMechanism lockingMechanism) : this(lockingMechanism, default) { }

        public SynchronizedValue(T value) : this(DefaultLockingMechanism(), value) { }

        public SynchronizedValue(ILockingMechanism lockingMechanism, T value)
        {
            Ensure.IsNotNull(lockingMechanism, nameof(lockingMechanism));

            _lockingMechanism = lockingMechanism;
            _value = value;
        }

        private static ILockingMechanism DefaultLockingMechanism()
        {
            return new LockSlimLockingMechanism();
        }

        public void SetValue(Func<T, T> setValue)
        {
            _lockingMechanism.Write(() => _value = setValue(_value));
        }

        public T Value
        {
            get => _lockingMechanism.Read(() => _value);
            set => _lockingMechanism.Write(() => _value = value);
        }
    }
}