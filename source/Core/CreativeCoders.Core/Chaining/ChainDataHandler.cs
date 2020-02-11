using System;

namespace CreativeCoders.Core.Chaining
{
    public class ChainDataHandler<TData, TResult> : IChainDataHandler<TData, TResult>
    {
        private readonly Func<TData, HandleResult<TResult>> _handle;

        public ChainDataHandler(Func<TData, HandleResult<TResult>> handle)
        {
            _handle = handle;
        }

        public HandleResult<TResult> Handle(TData data)
        {
            return _handle(data);
        }
    }
}