using System;
using System.Threading;

namespace CreativeCoders.Core.Threading
{
    public static class LockExtension
    {
        public static void Read(this ReaderWriterLockSlim self, Action action)
        {
            using (new AcquireReaderLock(self))
            {
                action();
            }
        }

        public static void Read(this ReaderWriterLockSlim self, Action action, int timeout)
        {
            using (new AcquireReaderLock(self, timeout))
            {
                action();
            }
        }

        public static T Read<T>(this ReaderWriterLockSlim self, Func<T> func)
        {
            using (new AcquireReaderLock(self))
            {
                return func();
            }
        }

        public static T Read<T>(this ReaderWriterLockSlim self, Func<T> func, int timeout)
        {
            using (new AcquireReaderLock(self, timeout))
            {
                return func();
            }
        }

        public static void Write(this ReaderWriterLockSlim self, Action action)
        {
            using (new AcquireWriterLock(self))
            {
                action();
            }
        }

        public static void Write(this ReaderWriterLockSlim self, Action action, int timeout)
        {
            using (new AcquireWriterLock(self, timeout))
            {
                action();
            }
        }

        public static T Write<T>(this ReaderWriterLockSlim self, Func<T> func)
        {
            using (new AcquireWriterLock(self))
            {
                return func();
            }
        }

        public static T Write<T>(this ReaderWriterLockSlim self, Func<T> func, int timeout)
        {
            using (new AcquireWriterLock(self, timeout))
            {
                return func();
            }
        }

        public static void Lock(this object self, Action action)
        {
            lock (self)
            {
                action();
            }
        }

        public static T Lock<T>(this object self, Func<T> func)
        {
            lock (self)
            {
                return func();
            }
        }
    }
}
