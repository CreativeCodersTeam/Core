using JetBrains.Annotations;

namespace CreativeCoders.Core.Executing;

[PublicAPI]
public interface IExecutable
{
    void Execute();
}