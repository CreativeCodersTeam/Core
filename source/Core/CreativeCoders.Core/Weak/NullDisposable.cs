using System;

namespace CreativeCoders.Core.Weak
{
    public class NullDisposable : IDisposable
    {
        public void Dispose() { }
    }
}