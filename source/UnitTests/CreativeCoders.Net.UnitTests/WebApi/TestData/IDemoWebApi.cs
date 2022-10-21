using System.Collections.Generic;
using System.Threading.Tasks;
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
}
