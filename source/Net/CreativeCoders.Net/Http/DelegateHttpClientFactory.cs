using System;
using System.Net.Http;
using CreativeCoders.Core;

namespace CreativeCoders.Net.Http;

///-------------------------------------------------------------------------------------------------
/// <summary>
///     A delegate HTTP client factory, which delegates the creation of a
///     <see cref="HttpClient"/> to a function.
/// </summary>
///
/// <seealso cref="IHttpClientFactory"/>
///-------------------------------------------------------------------------------------------------
public class DelegateHttpClientFactory : IHttpClientFactory
{
    private readonly Func<string, HttpClient> _createClient;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the CreativeCoders.Net.Http.DelegateHttpClientFactory
    ///     class.
    /// </summary>
    ///
    /// <param name="createClient"> The function for creating a new client. </param>
    ///-------------------------------------------------------------------------------------------------
    public DelegateHttpClientFactory(Func<string, HttpClient> createClient)
    {
        Ensure.IsNotNull(createClient, nameof(createClient));

        _createClient = createClient;
    }

    ///<inheritdoc/>
    public HttpClient CreateClient(string name)
    {
        return _createClient(name);
    }
}
