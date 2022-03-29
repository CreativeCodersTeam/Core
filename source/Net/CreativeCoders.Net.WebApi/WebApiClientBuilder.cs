using System;
using CreativeCoders.Net.WebApi.Building;
using CreativeCoders.Net.WebApi.Serialization;
using CreativeCoders.Net.WebApi.Serialization.Json;

namespace CreativeCoders.Net.WebApi;

public class WebApiClientBuilder<T> : IWebApiClientBuilder<T>
    where T : class
{
    private readonly IApiBuilder _apiBuilder;

    public WebApiClientBuilder(IApiBuilder apiBuilder)
    {
        _apiBuilder = apiBuilder;
    }

    public T Build(string baseUri)
    {
        if (!typeof(T).IsInterface)
        {
            throw new ArgumentException("Only interfaces are allowed");
        }

        return _apiBuilder.BuildApi<T>(baseUri, DefaultDataFormatter);
    }

    public IDataFormatter DefaultDataFormatter { get; set; } = new JsonDataFormatter();
}