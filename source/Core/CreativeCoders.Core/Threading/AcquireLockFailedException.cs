using System;

namespace CreativeCoders.Core.Threading
{
    public class AcquireLockFailedException : ApplicationException
    {
        public AcquireLockFailedException(string message, int timeout) : base(message)
        {
            Timeout = timeout;
        }

        public int Timeout { get; }
    }
}
