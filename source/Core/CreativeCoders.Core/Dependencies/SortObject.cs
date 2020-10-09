using System.Collections.Generic;
using System.Linq;

namespace CreativeCoders.Core.Dependencies {
    internal class SortObject<T>
        where T : class
    {
        public SortObject(DependencyObject<T> dependencyObject)
        {
            Element = dependencyObject.Element;
            DependsOn = dependencyObject.DependsOn.Select(x => x.Element).ToList();
        }

        public IList<T> DependsOn { get; }

        public T Element { get; }
    }
}