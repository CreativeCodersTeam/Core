using CreativeCoders.Net.WebApi.Serialization;

namespace CreativeCoders.Net.WebApi.Building;

public interface IApiBuilder
{
    T BuildApi<T>(string baseUri, IDataFormatter defaultDataFormatter)
        where T : class;
}