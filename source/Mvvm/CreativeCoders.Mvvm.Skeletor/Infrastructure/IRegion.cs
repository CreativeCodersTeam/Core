using System.Collections.Generic;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Skeletor.Infrastructure
{
    [PublicAPI]
    public interface IRegion
    {
        void AddView(object view);

        void RemoveView(object view);

        IEnumerable<object> Views { get; }
    }
}
