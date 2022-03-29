using System;
using System.Threading.Tasks;
using CreativeCoders.Core.Caching;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Caching;

public class CachedValueTests
{
    [Fact]
    public void Value_GetOrAddMode_ReturnsValueFromCache()
    {
        Func<string> getValue = () => "Test1234";

        var cache = A.Fake<ICache<int, string>>(x => x.Strict());
        A.CallTo(() => cache.GetOrAdd(1, getValue, CacheExpirationPolicy.NeverExpire, null))
            .Returns(getValue());

        var cachedValue = new CachedValue<int, string>(cache, 1, getValue, CacheExpirationPolicy.NeverExpire);

        var value = cachedValue.Value;

        Assert.Equal("Test1234", value);
        A.CallTo(() => cache.GetOrAdd(1, getValue, CacheExpirationPolicy.NeverExpire, null))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void Value_GetOrAddModeWithRegions_ReturnsValueFromCache()
    {
        Func<string> getValue = () => "Test1234";

        var cache = A.Fake<ICache<int, string>>(x => x.Strict());
        A.CallTo(() => cache.GetOrAdd(1, getValue, CacheExpirationPolicy.NeverExpire, "Region1"))
            .Returns(getValue());

        var cachedValue =
            new CachedValue<int, string>(cache, 1, getValue, CacheExpirationPolicy.NeverExpire, "Region1");

        var value = cachedValue.Value;

        Assert.Equal("Test1234", value);
        A.CallTo(() => cache.GetOrAdd(1, getValue, CacheExpirationPolicy.NeverExpire, "Region1"))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetValueAsync_GetOrAddMode_ReturnsValueFromCache()
    {
        Func<string> getValue = () => "Test1234";

        var cache = A.Fake<ICache<int, string>>(x => x.Strict());
        A.CallTo(() => cache.GetOrAddAsync(1, getValue, CacheExpirationPolicy.NeverExpire, null))
            .Returns(Task.FromResult(getValue()));

        var cachedValue = new CachedValue<int, string>(cache, 1, getValue, CacheExpirationPolicy.NeverExpire);

        var value = await cachedValue.GetValueAsync();

        Assert.Equal("Test1234", value);
        A.CallTo(() => cache.GetOrAddAsync(1, getValue, CacheExpirationPolicy.NeverExpire, null))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetValueAsync_GetOrAddModeWithRegions_ReturnsValueFromCache()
    {
        Func<string> getValue = () => "Test1234";

        var cache = A.Fake<ICache<int, string>>(x => x.Strict());
        A.CallTo(() => cache.GetOrAddAsync(1, getValue, CacheExpirationPolicy.NeverExpire, "Region1"))
            .Returns(Task.FromResult(getValue()));

        var cachedValue =
            new CachedValue<int, string>(cache, 1, getValue, CacheExpirationPolicy.NeverExpire, "Region1");

        var value = await cachedValue.GetValueAsync();

        Assert.Equal("Test1234", value);
        A.CallTo(() => cache.GetOrAddAsync(1, getValue, CacheExpirationPolicy.NeverExpire, "Region1"))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void Value_GetValueMode_ReturnsValueFromCache()
    {
        const string expectedValue = "Test1234";
        var outValue = string.Empty;

        var cache = A.Fake<ICache<int, string>>(x => x.Strict());
        A.CallTo(() => cache.TryGet(1, out outValue, null))
            .Returns(true)
            .AssignsOutAndRefParameters(expectedValue);

        var cachedValue = new CachedValue<int, string>(cache, 1);

        var value = cachedValue.Value;

        Assert.Equal(expectedValue, value);
        A.CallTo(() => cache.TryGet(1, out outValue, null))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void Value_GetValueModeWithRegions_ReturnsValueFromCache()
    {
        const string expectedValue = "Test1234";
        var outValue = string.Empty;

        var cache = A.Fake<ICache<int, string>>(x => x.Strict());
        A.CallTo(() => cache.TryGet(1, out outValue, "region1"))
            .Returns(true)
            .AssignsOutAndRefParameters(expectedValue);

        var cachedValue = new CachedValue<int, string>(cache, 1, "region1");

        var value = cachedValue.Value;

        Assert.Equal(expectedValue, value);
        A.CallTo(() => cache.TryGet(1, out outValue, "region1"))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetValueAsync_GetValueMode_ReturnsValueFromCache()
    {
        const string expectedValue = "Test1234";

        var cache = A.Fake<ICache<int, string>>(x => x.Strict());
        A.CallTo(() => cache.TryGetAsync(1, null))
            .Returns(Task.FromResult(new CacheRequestResult<string>(true, expectedValue)));

        var cachedValue = new CachedValue<int, string>(cache, 1);

        var value = await cachedValue.GetValueAsync();

        Assert.Equal(expectedValue, value);
        A.CallTo(() => cache.TryGetAsync(1, null))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetValueAsync_GetValueModeWithRegions_ReturnsValueFromCache()
    {
        const string expectedValue = "Test1234";

        var cache = A.Fake<ICache<int, string>>(x => x.Strict());
        A.CallTo(() => cache.TryGetAsync(1, "region1"))
            .Returns(new CacheRequestResult<string>(true, expectedValue));

        var cachedValue = new CachedValue<int, string>(cache, 1, "region1");

        var value = await cachedValue.GetValueAsync();

        Assert.Equal(expectedValue, value);
        A.CallTo(() => cache.TryGetAsync(1, "region1"))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void Value_GetValueOrDefaultMode_ReturnsValueFromCache()
    {
        const string expectedValue = "Test1234";
        var outValue = string.Empty;

        var cache = A.Fake<ICache<int, string>>(x => x.Strict());
        A.CallTo(() => cache.TryGet(1, out outValue, null))
            .Returns(true)
            .AssignsOutAndRefParameters(expectedValue);

        var cachedValue = new CachedValue<int, string>(cache, 1, "Text", null);

        var value = cachedValue.Value;

        Assert.Equal(expectedValue, value);
        A.CallTo(() => cache.TryGet(1, out outValue, null))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void Value_GetValueOrDefaultModeWithRegions_ReturnsValueFromCache()
    {
        const string expectedValue = "Test1234";
        var outValue = string.Empty;

        var cache = A.Fake<ICache<int, string>>(x => x.Strict());
        A.CallTo(() => cache.TryGet(1, out outValue, "region1"))
            .Returns(true)
            .AssignsOutAndRefParameters(expectedValue);

        var cachedValue = new CachedValue<int, string>(cache, 1, "Text", "region1");

        var value = cachedValue.Value;

        Assert.Equal(expectedValue, value);
        A.CallTo(() => cache.TryGet(1, out outValue, "region1"))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetValueAsync_GetValueOrDefaultMode_ReturnsValueFromCache()
    {
        const string expectedValue = "Test1234";

        var cache = A.Fake<ICache<int, string>>(x => x.Strict());
        A.CallTo(() => cache.TryGetAsync(1, null))
            .Returns(Task.FromResult(new CacheRequestResult<string>(true, expectedValue)));

        var cachedValue = new CachedValue<int, string>(cache, 1, "Text", null);

        var value = await cachedValue.GetValueAsync();

        Assert.Equal(expectedValue, value);
        A.CallTo(() => cache.TryGetAsync(1, null))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetValueAsync_GetValueOrDefaultModeWithRegions_ReturnsValueFromCache()
    {
        const string expectedValue = "Test1234";

        var cache = A.Fake<ICache<int, string>>(x => x.Strict());
        A.CallTo(() => cache.TryGetAsync(1, "region1"))
            .Returns(Task.FromResult(new CacheRequestResult<string>(true, expectedValue)));

        var cachedValue = new CachedValue<int, string>(cache, 1, "Text", "region1");

        var value = await cachedValue.GetValueAsync();

        Assert.Equal(expectedValue, value);
        A.CallTo(() => cache.TryGetAsync(1, "region1"))
            .MustHaveHappenedOnceExactly();
    }
}
