namespace CreativeCoders.Core.Chaining;

public interface IChainDataHandler<in TData, TResult>
{
    HandleResult<TResult> Handle(TData data);
}
