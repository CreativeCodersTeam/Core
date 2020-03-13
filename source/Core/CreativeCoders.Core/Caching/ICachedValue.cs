using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Caching
{
    [PublicAPI]
    public interface ICachedValue<TValue>
    {
        Task<TValue> GetValueAsync();
        
        TValue Value { get; }
    }
}