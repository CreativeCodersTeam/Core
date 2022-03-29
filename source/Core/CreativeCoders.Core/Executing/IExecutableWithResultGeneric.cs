using JetBrains.Annotations;

namespace CreativeCoders.Core.Executing;

[PublicAPI]
public interface IExecutableWithResult<in TParameter, out TResult>
{
    TResult Execute(TParameter parameter);
}