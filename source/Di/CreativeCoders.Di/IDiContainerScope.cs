using System;

namespace CreativeCoders.Di
{
    public interface IDiContainerScope : IDisposable
    {
        IDiContainer Container { get; }
    }
}