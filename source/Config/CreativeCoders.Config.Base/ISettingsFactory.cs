using System.Collections.Generic;

namespace CreativeCoders.Config.Base;

public interface ISettingsFactory<out T>
    where T : class
{
    IEnumerable<T> Create();
}
