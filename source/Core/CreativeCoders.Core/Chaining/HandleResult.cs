using JetBrains.Annotations;

namespace CreativeCoders.Core.Chaining;

[PublicAPI]
public class HandleResult<T>
{
    public HandleResult(bool isHandled, T result)
    {
        IsHandled = isHandled;
        Result = result;
    }
        
    public bool IsHandled { get; }

    public T Result { get; }
}