using System;
using System.Linq;
using CreativeCoders.Core.Collections;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Ribbon;

[PublicAPI]
public class RibbonItemCollection<T> : ExtendedObservableCollection<T> where T: RibbonItemViewModel
{
    public RibbonItemViewModel FindByName(string name)
    {
        return this.FirstOrDefault(item => item.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
    }
}