using System.Collections.Generic;

namespace CreativeCoders.Mvvm.Skeletor.Infrastructure
{
    public interface IRegion
    {
        void AddView(object view);

        void RemoveView(object view);

        IEnumerable<object> Views { get; }
    }
}
