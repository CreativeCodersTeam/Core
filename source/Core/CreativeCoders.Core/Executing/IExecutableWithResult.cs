using JetBrains.Annotations;

namespace CreativeCoders.Core.Executing;

[PublicAPI]
public interface IExecutableWithResult<out T>
{
    T Execute();
}
