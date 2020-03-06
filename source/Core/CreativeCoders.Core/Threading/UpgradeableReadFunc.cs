using System;

namespace CreativeCoders.Core.Threading
{
    public delegate T UpgradeableReadFunc<T>(Func<IDisposable> useWriteLock);
}