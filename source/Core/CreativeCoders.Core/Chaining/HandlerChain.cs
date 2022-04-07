using System.Collections.Generic;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Chaining;

[PublicAPI]
public class HandlerChain<TData, TResult>
{
    private readonly IEnumerable<IChainDataHandler<TData, TResult>> _handlers;

    public HandlerChain(IEnumerable<IChainDataHandler<TData, TResult>> handlers)
    {
        _handlers = handlers;
    }

    public TResult Handle(TData data)
    {
        return Handle(data, default);
    }

    public TResult Handle(TData data, TResult defaultResult)
    {
        // ReSharper disable once LoopCanBePartlyConvertedToQuery
        foreach (var handler in _handlers)
        {
            var handleResult = handler.Handle(data);

            if (handleResult.IsHandled)
            {
                return handleResult.Result;
            }
        }

        return defaultResult;
    }
}
