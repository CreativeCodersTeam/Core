using System;
using CreativeCoders.Core;

namespace CreativeCoders.Di
{
    public class DiContainerScope : IDiContainerScope
    {
        private readonly Action _disposeCallback;

        private bool _disposed;

        public DiContainerScope(IDiContainer container, Action disposeCallback)
        {
            Ensure.IsNotNull(container, nameof(container));
            Ensure.IsNotNull(disposeCallback, nameof(disposeCallback));

            Container = container;
            _disposeCallback = disposeCallback;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _disposeCallback();
        }

        public IDiContainer Container { get; }
    }
}