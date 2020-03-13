using System;

namespace CreativeCoders.Core.Threading
{
    public delegate void UpgradeableReadAction(Func<IDisposable> useWriteLock);
}