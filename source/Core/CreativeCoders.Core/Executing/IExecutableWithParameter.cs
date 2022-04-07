using JetBrains.Annotations;

namespace CreativeCoders.Core.Executing;

[PublicAPI]
public interface IExecutableWithParameter
{
    void Execute(object parameter);
}
