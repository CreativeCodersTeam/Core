using System.Collections.Generic;

namespace CreativeCoders.Config.Base
{
    public interface ISettings<out T>
        where T : class
    {
        IEnumerable<T> Values { get; }
    }
}