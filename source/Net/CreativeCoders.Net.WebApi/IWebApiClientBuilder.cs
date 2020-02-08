using CreativeCoders.Net.WebApi.Serialization;
using JetBrains.Annotations;

namespace CreativeCoders.Net.WebApi
{
    [PublicAPI]
    public interface IWebApiClientBuilder<out T>
        where T : class
    {
        T Build(string baseUri);

        IDataFormatter DefaultDataFormatter { get; set; }
    }
}