using System;

namespace CreativeCoders.Core.Threading
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A locking mechanism implementation that does no locking. Can be used in single threaded
    ///     environments.
    /// </summary>
    ///
    /// <seealso cref="ILockingMechanism"/>
    ///-------------------------------------------------------------------------------------------------
    public class NoLockingMechanism : ILockingMechanism
    {
        ///<inheritdoc />
        public void Read(Action action)
        {
            action();
        }

        ///<inheritdoc />
        public T Read<T>(Func<T> function)
        {
            return function();
        }

        ///<inheritdoc />
        public void Write(Action action)
        {
            action();
        }

        ///<inheritdoc />
        public T Write<T>(Func<T> function)
        {
            return function();
        }
    }
}