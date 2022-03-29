using JetBrains.Annotations;

namespace CreativeCoders.Core.Executing;

[PublicAPI]
public interface IExecutable<in T>
{
    void Execute(T parameter);
}