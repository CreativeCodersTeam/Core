using System.Collections.Generic;
using System.Threading.Tasks;
using CreativeCoders.Net.WebApi;
using CreativeCoders.Net.WebApi.Definition;

namespace CreativeCoders.Net.UnitTests.WebApi.TestData;

public interface IDemoWebApi
{
    [Get("items")]
    Task<IEnumerable<DemoItem>> GetItemsAsync();

    [Get("one-item")]
    Task<DemoItem> GetItemAsync();

    [Get("item/{itemId}")]
    Task<DemoItem> GetItemAsync(string itemId);

    [Post("create")]
    Task<DemoItem> CreateItemAsync([ViaBody] DemoItem item);

    [Get("one-header")]
    [Header("TestHeader", "HeaderValue0")]
    Task GetWithOneHeaderAsync();

    [Get("two-headers")]
    [Header("TheHeader", "HeaderValueOne")]
    [Header("ThatHeader")]
    Task GetWithTwoHeadersAsync();

    [Get("misc/item-with-response")]
    Task<Response<DemoItem>> GetItemResponseAsync();
}
