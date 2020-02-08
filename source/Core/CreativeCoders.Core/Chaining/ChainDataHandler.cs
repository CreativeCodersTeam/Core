using System;

namespace CreativeCoders.Core.Chaining
{
    public class ChainDataHandler<TData, TResult> : IChainDataHandler<TData, TResult>
    {
        private readonly Func<TData, HandleResult<TResult>> _handleFunc;

        public ChainDataHandler(Func<TData, HandleResult<TResult>> handleFunc)
        {
            _handleFunc = handleFunc;
        }

        public HandleResult<TResult> Handle(TData data)
        {
            return _handleFunc(data);
        }
    }
}