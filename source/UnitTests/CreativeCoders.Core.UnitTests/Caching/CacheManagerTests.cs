using CreativeCoders.Core.Caching;
using CreativeCoders.Core.Caching.Default;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Caching;

public class CacheManagerTests
{
    [Fact]
    public void GetCache_Call_ReturnsCache()
    {
        var cache = CacheManager.CreateCache<int, string>();

        Assert.NotNull(cache);
        Assert.IsAssignableFrom<ICache<int, string>>(cache);
    }
}
