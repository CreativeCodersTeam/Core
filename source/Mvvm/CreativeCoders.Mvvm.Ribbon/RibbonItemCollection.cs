using System;
using System.Linq;
using CreativeCoders.Core.Collections;

namespace CreativeCoders.Mvvm.Ribbon
{
    public class RibbonItemCollection<T> : ExtendedObservableCollection<T> where T: RibbonItemViewModel
    {
        public RibbonItemViewModel FindByName(string name)
        {
            return this.FirstOrDefault(item => item.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
