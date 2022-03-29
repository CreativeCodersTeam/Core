using System;

namespace CreativeCoders.Core.Threading;

public delegate T UpgradeableReadFunc<out T>(Func<IDisposable> useWriteLock);
