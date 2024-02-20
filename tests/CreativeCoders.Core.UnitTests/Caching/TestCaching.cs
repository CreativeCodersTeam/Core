using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CreativeCoders.Core.Caching;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Caching;

[SuppressMessage("ReSharper", "MethodHasAsyncOverload")]
internal static class TestCaching
{
    public static void TryGet_KeyNotExists_ReturnFalse(ICache<int, string> cache)
    {
        var keyExists = cache.TryGet(1, out _);

        Assert.False(keyExists);
    }

    public static void TryGet_KeyNotExistsWithRegion_ReturnFalse(ICache<int, string> cache)
    {
        cache.AddOrUpdate(1, "Test", "region1");

        Assert.False(cache.TryGet(1, out _));
        Assert.False(cache.TryGet(1, out _, "region2"));
    }

    public static async Task TryGetAsync_KeyNotExists_ReturnFalse(ICache<int, string> cache)
    {
        var cacheRequestResult = await cache.TryGetAsync(1);

        Assert.False(cacheRequestResult.EntryExists);
    }

    public static async Task TryGetAsync_KeyNotExistsWithRegion_ReturnFalse(ICache<int, string> cache)
    {
        cache.AddOrUpdate(1, "Test", "region1");

        var cacheRequestResult = await cache.TryGetAsync(1);

        Assert.False(cacheRequestResult.EntryExists);
    }

    public static void TryGet_KeyExists_ReturnsTrueAndValue(ICache<int, string> cache)
    {
        cache.AddOrUpdate(1, "TestValue");

        var keyExists = cache.TryGet(1, out var value);

        Assert.True(keyExists);
        Assert.Equal("TestValue", value);
    }

    public static void TryGet_KeyExistsWithRegion_ReturnsTrueAndValue(ICache<int, string> cache)
    {
        cache.AddOrUpdate(1, "TestValue", "region1");
        cache.AddOrUpdate(1, "TestValue2", "region2");

        var keyExists = cache.TryGet(1, out var value, "region1");

        Assert.True(keyExists);
        Assert.Equal("TestValue", value);

        var keyExists2 = cache.TryGet(1, out var value2, "region2");

        Assert.True(keyExists2);
        Assert.Equal("TestValue2", value2);
    }

    public static async Task TryGetAsync_KeyExists_ReturnsTrueAndValue(ICache<int, string> cache)
    {
        await cache.AddOrUpdateAsync(1, "TestValue");

        var cacheRequestResult = await cache.TryGetAsync(1);

        Assert.True(cacheRequestResult.EntryExists);
        Assert.Equal("TestValue", cacheRequestResult.Value);
    }

    public static async Task TryGetAsync_KeyExistsWithRegion_ReturnsTrueAndValue(ICache<int, string> cache)
    {
        await cache.AddOrUpdateAsync(1, "TestValue", "region1");
        await cache.AddOrUpdateAsync(1, "TestValue2", "region2");

        var cacheRequestResult = await cache.TryGetAsync(1, "region1");

        Assert.True(cacheRequestResult.EntryExists);
        Assert.Equal("TestValue", cacheRequestResult.Value);
    }

    public static void TryGet_AddOrUpdateTwoValue_ReturnsLastValue(ICache<int, string> cache)
    {
        cache.AddOrUpdate(1, "TestValue");
        cache.AddOrUpdate(1, "TestValue1234");

        var keyExists = cache.TryGet(1, out var value);

        Assert.True(keyExists);
        Assert.Equal("TestValue1234", value);
    }

    public static void TryGet_AddOrUpdateTwoValueWithRegions_ReturnsLastValue(ICache<int, string> cache)
    {
        cache.AddOrUpdate(1, "TestValue", "region1");
        cache.AddOrUpdate(1, "TestValue1234", "region1");

        var keyExists = cache.TryGet(1, out var value, "region1");

        Assert.True(keyExists);
        Assert.Equal("TestValue1234", value);
    }

    public static async Task TryGetAsync_AddOrUpdateTwoValue_ReturnsLastValue(ICache<int, string> cache)
    {
        await cache.AddOrUpdateAsync(1, "TestValue");
        await cache.AddOrUpdateAsync(1, "TestValue1234");

        var cacheRequestResult = await cache.TryGetAsync(1);

        Assert.True(cacheRequestResult.EntryExists);
        Assert.Equal("TestValue1234", cacheRequestResult.Value);
    }

    public static async Task TryGetAsync_AddOrUpdateTwoValueWithRegions_ReturnsLastValue(
        ICache<int, string> cache)
    {
        await cache.AddOrUpdateAsync(1, "TestValue", "region1");
        await cache.AddOrUpdateAsync(1, "TestValue1234", "region1");

        var cacheRequestResult = await cache.TryGetAsync(1, "region1");

        Assert.True(cacheRequestResult.EntryExists);
        Assert.Equal("TestValue1234", cacheRequestResult.Value);
    }

    public static void Clear_TryGetValue_ReturnFalse(ICache<int, string> cache)
    {
        cache.AddOrUpdate(1, "TestValue1");
        cache.AddOrUpdate(2, "TestValue2");
        cache.Clear();

        var keyExists1 = cache.TryGet(1, out _);
        var keyExists2 = cache.TryGet(2, out _);

        Assert.False(keyExists1);
        Assert.False(keyExists2);
    }

    public static void Clear_TryGetValueWithRegions_ReturnFalse(ICache<int, string> cache)
    {
        cache.AddOrUpdate(1, "TestValue1", "region1");
        cache.AddOrUpdate(2, "TestValue2", "region2");
        cache.Clear("region1");

        var keyExists1 = cache.TryGet(1, out _, "region1");
        var keyExists2 = cache.TryGet(2, out _, "region2");

        Assert.False(keyExists1);
        Assert.True(keyExists2);
        Assert.Equal("TestValue2", cache.GetValue(2, "region2"));
    }

    public static async Task ClearAsync_TryGetValue_ReturnFalse(ICache<int, string> cache)
    {
        await cache.AddOrUpdateAsync(1, "TestValue1");
        await cache.AddOrUpdateAsync(2, "TestValue2");
        await cache.ClearAsync();

        var cacheRequestResult1 = await cache.TryGetAsync(1);
        var cacheRequestResult2 = await cache.TryGetAsync(2);

        Assert.False(cacheRequestResult1.EntryExists);
        Assert.False(cacheRequestResult2.EntryExists);
    }

    public static async Task ClearAsync_TryGetValueWithRegions_ReturnFalse(ICache<int, string> cache)
    {
        await cache.AddOrUpdateAsync(1, "TestValue1", "region1");
        await cache.AddOrUpdateAsync(2, "TestValue2", "region2");
        await cache.ClearAsync("region1");

        var cacheRequestResult1 = await cache.TryGetAsync(1, "region1");
        var cacheRequestResult2 = await cache.TryGetAsync(2, "region2");

        Assert.False(cacheRequestResult1.EntryExists);
        Assert.True(cacheRequestResult2.EntryExists);
    }

    public static void Remove_TryGetValue_ReturnFalse(ICache<int, string> cache)
    {
        cache.AddOrUpdate(1, "TestValue1");
        cache.AddOrUpdate(2, "TestValue2");
        cache.Remove(1);

        var keyExists1 = cache.TryGet(1, out _);
        var keyExists2 = cache.TryGet(2, out _);

        Assert.False(keyExists1);
        Assert.True(keyExists2);
    }

    public static void Remove_WithOutRegionsTryGetValue_ReturnFalse(ICache<int, string> cache)
    {
        cache.AddOrUpdate(1, "TestValue1", "region1");
        cache.AddOrUpdate(1, "TestValue1", "region2");
        cache.AddOrUpdate(2, "TestValue2", "region1");
        cache.AddOrUpdate(2, "TestValue2", "region2");
        cache.Remove(1);

        var keyExists1 = cache.TryGet(1, out _, "region1");
        var keyExists2 = cache.TryGet(1, out _, "region2");
        var keyExists3 = cache.TryGet(2, out _, "region1");
        var keyExists4 = cache.TryGet(2, out _, "region2");

        Assert.True(keyExists1);
        Assert.True(keyExists2);
        Assert.True(keyExists3);
        Assert.True(keyExists4);
    }

    public static void Remove_WithRegionsTryGetValue_ReturnFalse(ICache<int, string> cache)
    {
        cache.AddOrUpdate(1, "TestValue1", "region1");
        cache.AddOrUpdate(1, "TestValue1", "region2");
        cache.AddOrUpdate(2, "TestValue2", "region1");
        cache.AddOrUpdate(2, "TestValue2", "region2");
        cache.Remove(1, "region1");

        var keyExists1 = cache.TryGet(1, out _, "region1");
        var keyExists2 = cache.TryGet(1, out _, "region2");
        var keyExists3 = cache.TryGet(2, out _, "region1");
        var keyExists4 = cache.TryGet(2, out _, "region2");

        Assert.False(keyExists1);
        Assert.True(keyExists2);
        Assert.True(keyExists3);
        Assert.True(keyExists4);
    }

    public static async Task RemoveAsync_TryGetValue_ReturnFalse(ICache<int, string> cache)
    {
        await cache.AddOrUpdateAsync(1, "TestValue");
        await cache.AddOrUpdateAsync(2, "TestValue2");
        await cache.RemoveAsync(1);

        var cacheRequestResult1 = await cache.TryGetAsync(1);
        var cacheRequestResult2 = await cache.TryGetAsync(2);

        Assert.False(cacheRequestResult1.EntryExists);
        Assert.True(cacheRequestResult2.EntryExists);
    }

    public static async Task RemoveAsync_WithOutRegionsTryGetValue_ReturnFalse(ICache<int, string> cache)
    {
        await cache.AddOrUpdateAsync(1, "TestValue1", "region1");
        await cache.AddOrUpdateAsync(1, "TestValue1", "region2");
        await cache.AddOrUpdateAsync(2, "TestValue2", "region1");
        await cache.AddOrUpdateAsync(2, "TestValue2", "region2");
        await cache.RemoveAsync(1);

        var cacheRequestResult1 = await cache.TryGetAsync(1, "region1");
        var cacheRequestResult2 = await cache.TryGetAsync(1, "region2");
        var cacheRequestResult3 = await cache.TryGetAsync(2, "region1");
        var cacheRequestResult4 = await cache.TryGetAsync(2, "region2");

        Assert.True(cacheRequestResult1.EntryExists);
        Assert.True(cacheRequestResult2.EntryExists);
        Assert.True(cacheRequestResult3.EntryExists);
        Assert.True(cacheRequestResult4.EntryExists);
    }

    public static async Task RemoveAsync_WithRegionsTryGetValue_ReturnFalse(ICache<int, string> cache)
    {
        await cache.AddOrUpdateAsync(1, "TestValue1", "region1");
        await cache.AddOrUpdateAsync(1, "TestValue1", "region2");
        await cache.AddOrUpdateAsync(2, "TestValue2", "region1");
        await cache.AddOrUpdateAsync(2, "TestValue2", "region2");
        await cache.RemoveAsync(1, "region1");

        var cacheRequestResult1 = await cache.TryGetAsync(1, "region1");
        var cacheRequestResult2 = await cache.TryGetAsync(1, "region2");
        var cacheRequestResult3 = await cache.TryGetAsync(2, "region1");
        var cacheRequestResult4 = await cache.TryGetAsync(2, "region2");

        Assert.False(cacheRequestResult1.EntryExists);
        Assert.True(cacheRequestResult2.EntryExists);
        Assert.True(cacheRequestResult3.EntryExists);
        Assert.True(cacheRequestResult4.EntryExists);
    }

    public static void GetValue_GetExisting_ReturnsValue(ICache<int, string> cache)
    {
        cache.AddOrUpdate(1, "TestValue");

        var value = cache.GetValue(1);

        Assert.Equal("TestValue", value);
    }

    public static void GetValue_GetExistingWithRegion_ReturnsValue(ICache<int, string> cache)
    {
        cache.AddOrUpdate(1, "TestValue", "region1");
        cache.AddOrUpdate(1, "TestValue2", "region2");

        var value = cache.GetValue(1, "region1");
        var value2 = cache.GetValue(1, "region2");

        Assert.Equal("TestValue", value);
        Assert.Equal("TestValue2", value2);
    }

    public static async Task GetValueAsync_GetExisting_ReturnsValue(ICache<int, string> cache)
    {
        await cache.AddOrUpdateAsync(1, "TestValue");

        var value = await cache.GetValueAsync(1);

        Assert.Equal("TestValue", value);
    }

    public static async Task GetValueAsync_GetExistingWithRegion_ReturnsValue(ICache<int, string> cache)
    {
        await cache.AddOrUpdateAsync(1, "TestValue", "region1");
        await cache.AddOrUpdateAsync(1, "TestValue2", "region2");

        var value = await cache.GetValueAsync(1, "region1");
        var value2 = await cache.GetValueAsync(1, "region2");

        Assert.Equal("TestValue", value);
        Assert.Equal("TestValue2", value2);
    }

    public static void GetValue_KeyNotExists_ThrowsException(ICache<int, string> cache)
    {
        var exception = Assert.Throws<CacheEntryNotFoundException>(() => cache.GetValue(1));

        Assert.Equal("1", exception.Key);
    }

    public static void GetValue_KeyNotExistsWithRegions_ThrowsException(ICache<int, string> cache)
    {
        cache.AddOrUpdate(1, "TestValue", "region1");

        var exception = Assert.Throws<CacheEntryNotFoundException>(() => cache.GetValue(1));

        Assert.Equal("1", exception.Key);
        Assert.Equal(string.Empty, exception.RegionName);

        var exception2 = Assert.Throws<CacheEntryNotFoundException>(() => cache.GetValue(1, "region2"));

        Assert.Equal("1", exception2.Key);
        Assert.Equal("region2", exception2.RegionName);
    }

    public static async Task GetValueAsync_KeyNotExists_ThrowsException(ICache<int, string> cache)
    {
        var exception =
            await Assert.ThrowsAsync<CacheEntryNotFoundException>(async () =>
                await cache.GetValueAsync(1));

        Assert.Equal("1", exception.Key);
    }

    public static async Task GetValueAsync_KeyNotExistsWithRegions_ThrowsException(ICache<int, string> cache)
    {
        cache.AddOrUpdate(1, "TestValue", "region1");

        var exception =
            await Assert.ThrowsAsync<CacheEntryNotFoundException>(async () =>
                await cache.GetValueAsync(1));

        Assert.Equal("1", exception.Key);
        Assert.Equal(string.Empty, exception.RegionName);

        var exception2 =
            await Assert.ThrowsAsync<CacheEntryNotFoundException>(async () =>
                await cache.GetValueAsync(1, "region2"));

        Assert.Equal("1", exception2.Key);
        Assert.Equal("region2", exception2.RegionName);
    }

    public static void GetValue_KeyNotExists_ReturnNull(ICache<int, string> cache)
    {
        var value = cache.GetValue(1, false);

        Assert.Null(value);
    }

    public static void GetValue_KeyNotExistsWithRegions_ReturnNull(ICache<int, string> cache)
    {
        cache.AddOrUpdate(1, "TestValue", "region1");

        var value = cache.GetValue(1, false);

        Assert.Null(value);
    }

    public static async Task GetValueAsync_KeyNotExists_ReturnNull(ICache<int, string> cache)
    {
        var value = await cache.GetValueAsync(1, false);

        Assert.Null(value);
    }

    public static async Task GetValueAsync_KeyNotExistsWithRegions_ReturnNull(ICache<int, string> cache)
    {
        cache.AddOrUpdate(1, "TestValue", "region1");

        var value = await cache.GetValueAsync(1, false);

        Assert.Null(value);
    }

    public static void GetValue_KeyNotExists_ReturnDefaultValue(ICache<int, string> cache)
    {
        var value = cache.GetValueOrDefault(1, "DefaultValue");

        Assert.Equal("DefaultValue", value);
    }

    public static async Task GetValueAsync_KeyNotExists_ReturnDefaultValue(ICache<int, string> cache)
    {
        var value = await cache.GetValueOrDefaultAsync(1, "DefaultValue");

        Assert.Equal("DefaultValue", value);
    }

    public static void GetValue_KeyNotExists_ReturnNewValueAndStoreToCache(ICache<int, string> cache)
    {
        var value = cache.GetOrAdd(1, () => "DefaultValue");
        var secondValue = cache.GetValue(1);

        Assert.Equal("DefaultValue", value);
        Assert.Equal("DefaultValue", secondValue);
    }

    public static async Task GetValueAsync_KeyNotExists_ReturnNewValueAndStoreToCache(
        ICache<int, string> cache)
    {
        var value = await cache.GetOrAddAsync(1, () => "DefaultValue");
        var secondValue = await cache.GetValueAsync(1);

        Assert.Equal("DefaultValue", value);
        Assert.Equal("DefaultValue", secondValue);
    }

    public static void GetValue_KeyExists_ReturnOldValue(ICache<int, string> cache)
    {
        var getValueCalled = 0;

        cache.AddOrUpdate(1, "TestValue");

        var value = cache.GetOrAdd(1, () =>
        {
            getValueCalled++;
            return "DefaultValue";
        });
        var secondValue = cache.GetValue(1);

        Assert.Equal("TestValue", value);
        Assert.Equal("TestValue", secondValue);
        Assert.Equal(0, getValueCalled);
    }

    public static async Task GetValueAsync_KeyExists_ReturnOldValue(ICache<int, string> cache)
    {
        var getValueCalled = 0;

        await cache.AddOrUpdateAsync(1, "TestValue");

        var value = await cache.GetOrAddAsync(1, () =>
        {
            getValueCalled++;
            return "DefaultValue";
        });
        var secondValue = await cache.GetValueAsync(1);

        Assert.Equal("TestValue", value);
        Assert.Equal("TestValue", secondValue);
        Assert.Equal(0, getValueCalled);
    }

    public static async Task TryGet_AfterExpiration_ReturnsFalse(ICache<int, string> cache)
    {
        var expirationPolicy = A.Fake<ICacheExpirationPolicy>();
        A.CallTo(() => expirationPolicy.ExpirationMode).Returns(CacheExpirationMode.AbsoluteDateTime);
        var expirationDateTime = DateTime.Now;
        A.CallTo(() => expirationPolicy.AbsoluteDateTime).Returns(expirationDateTime);

        cache.AddOrUpdate(1, "TestValue", expirationPolicy);

        await Task.Delay(100);

        Assert.False(cache.TryGet(1, out _));
    }

    public static async Task TryGetAsync_AfterExpiration_ReturnsFalse(ICache<int, string> cache)
    {
        var expirationPolicy = A.Fake<ICacheExpirationPolicy>();
        A.CallTo(() => expirationPolicy.ExpirationMode).Returns(CacheExpirationMode.AbsoluteDateTime);
        var expirationDateTime = DateTime.Now;
        A.CallTo(() => expirationPolicy.AbsoluteDateTime).Returns(expirationDateTime);

        await cache.AddOrUpdateAsync(1, "TestValue", expirationPolicy);

        await Task.Delay(100);

        Assert.False(cache.TryGet(1, out _));
    }

    public static async Task TryGet_AfterExpirationTimeSpan_ReturnsFalse(ICache<int, string> cache)
    {
        var expirationPolicy = A.Fake<ICacheExpirationPolicy>();
        A.CallTo(() => expirationPolicy.ExpirationMode).Returns(CacheExpirationMode.SlidingTimeSpan);

        A.CallTo(() => expirationPolicy.SlidingTimeSpan).Returns(TimeSpan.FromMilliseconds(50));

        cache.AddOrUpdate(1, "TestValue", expirationPolicy);

        await Task.Delay(100);

        Assert.False(cache.TryGet(1, out _));
    }

    public static async Task TryGetAsync_AfterExpirationTimeSpan_ReturnsFalse(ICache<int, string> cache)
    {
        var expirationPolicy = A.Fake<ICacheExpirationPolicy>();
        A.CallTo(() => expirationPolicy.ExpirationMode).Returns(CacheExpirationMode.SlidingTimeSpan);

        A.CallTo(() => expirationPolicy.SlidingTimeSpan).Returns(TimeSpan.FromMilliseconds(50));

        await cache.AddOrUpdateAsync(1, "TestValue", expirationPolicy);

        await Task.Delay(100);

        Assert.False(cache.TryGet(1, out _));
    }

    public static void GetOrAdd_TwoTimesCalled_ResultAlwaysTheSameAndGetValueFuncCalledOneTime(
        ICache<int, string> cache)
    {
        const string testValue = "Test1";

        var getValueCalledCount = 0;
        var value = cache.GetOrAdd(1, () =>
        {
            getValueCalledCount++;
            return testValue;
        });

        Assert.Equal(testValue, value);

        var secondValue = cache.GetOrAdd(1, () =>
        {
            getValueCalledCount++;
            return testValue;
        });

        Assert.Equal(testValue, secondValue);
        Assert.Equal(1, getValueCalledCount);
    }

    public static void GetOrAdd_TwoTimesCalledWithRegions_ResultAlwaysTheSameAndGetValueFuncCalledTwoTime(
        ICache<int, string> cache)
    {
        const string testValue = "Test1";

        var getValueCalledCount = 0;
        var value = cache.GetOrAdd(1, () =>
        {
            getValueCalledCount++;
            return testValue;
        }, "region1");

        Assert.Equal(testValue, value);

        var secondValue = cache.GetOrAdd(1, () =>
        {
            getValueCalledCount++;
            return testValue;
        }, "region2");

        Assert.Equal(testValue, secondValue);
        Assert.Equal(2, getValueCalledCount);
    }

    public static async Task GetOrAddAsync_TwoTimesCalled_ResultAlwaysTheSameAndGetValueFuncCalledOneTime(
        ICache<int, string> cache)
    {
        const string testValue = "Test1";

        var getValueCalledCount = 0;
        var value = await cache.GetOrAddAsync(1, () =>
        {
            getValueCalledCount++;
            return testValue;
        });

        Assert.Equal(testValue, value);

        var secondValue = await cache.GetOrAddAsync(1, () =>
        {
            getValueCalledCount++;
            return testValue;
        });

        Assert.Equal(testValue, secondValue);
        Assert.Equal(1, getValueCalledCount);
    }

    public static async Task
        GetOrAddAsync_TwoTimesCalledWithRegions_ResultAlwaysTheSameAndGetValueFuncCalledTwoTime(
            ICache<int, string> cache)
    {
        const string testValue = "Test1";

        var getValueCalledCount = 0;
        var value = await cache.GetOrAddAsync(1, () =>
        {
            getValueCalledCount++;
            return testValue;
        }, "region1");

        Assert.Equal(testValue, value);

        var secondValue = await cache.GetOrAddAsync(1, () =>
        {
            getValueCalledCount++;
            return testValue;
        }, "region2");

        Assert.Equal(testValue, secondValue);
        Assert.Equal(2, getValueCalledCount);
    }

    public static void GetOrAdd_TwoTimesCalledWithNeverExpire_ResultAlwaysTheSameAndGetValueFuncCalledOneTime(
        ICache<int, string> cache)
    {
        const string testValue = "Test1";

        var getValueCalledCount = 0;
        var value = cache.GetOrAdd(1, () =>
        {
            getValueCalledCount++;
            return testValue;
        }, CacheExpirationPolicy.NeverExpire);

        Assert.Equal(testValue, value);

        var secondValue = cache.GetOrAdd(1, () =>
        {
            getValueCalledCount++;
            return testValue;
        }, CacheExpirationPolicy.NeverExpire);

        Assert.Equal(testValue, secondValue);
        Assert.Equal(1, getValueCalledCount);
    }

    public static async Task
        GetOrAdd_TwoTimesCalledWithDateTimeExpire_ResultAlwaysTheSameAndGetValueFuncCalledTwoTime(
            ICache<int, string> cache)
    {
        const string testValue = "Test1";

        var getValueCalledCount = 0;
        var value = cache.GetOrAdd(1, () =>
        {
            getValueCalledCount++;
            return testValue;
        }, CacheExpirationPolicy.AfterAbsoluteDateTime(DateTime.Now.AddMilliseconds(50)));

        Assert.Equal(testValue, value);

        await Task.Delay(100);

        var secondValue = cache.GetOrAdd(1, () =>
        {
            getValueCalledCount++;
            return testValue;
        }, CacheExpirationPolicy.NeverExpire);

        Assert.Equal(testValue, secondValue);
        Assert.Equal(2, getValueCalledCount);
    }

    public static async Task
        GetOrAdd_TwoTimesCalledWithNoDateTimeExpire_ResultAlwaysTheSameAndGetValueFuncCalledOneTime(
            ICache<int, string> cache)
    {
        const string testValue = "Test1";

        var getValueCalled = false;
        var getValueCalled1 = false;
        var getValueCalled2 = false;
        var value = cache.GetOrAdd(1, () =>
        {
            getValueCalled = true;
            return testValue;
        }, CacheExpirationPolicy.AfterAbsoluteDateTime(DateTime.Now.AddMilliseconds(200)));

        Assert.Equal(testValue, value);

        await Task.Delay(50);

        var secondValue = cache.GetOrAdd(1, () =>
        {
            getValueCalled1 = true;
            return testValue;
        }, CacheExpirationPolicy.AfterAbsoluteDateTime(DateTime.Now.AddMilliseconds(200)));

        await Task.Delay(200);

        var thirdValue = cache.GetOrAdd(1, () =>
        {
            getValueCalled2 = true;
            return testValue;
        }, CacheExpirationPolicy.AfterAbsoluteDateTime(DateTime.Now.AddMilliseconds(200)));

        Assert.Equal(testValue, secondValue);
        Assert.Equal(testValue, thirdValue);
        Assert.True(getValueCalled);
        Assert.False(getValueCalled1);
        Assert.True(getValueCalled2);
    }

    public static async Task
        GetOrAdd_TwoTimesCalledWithNoTimeSpanExpire_ResultAlwaysTheSameAndGetValueFuncCalledTwoTimes(
            ICache<int, string> cache)
    {
        const string testValue = "Test1";

        var getValueCalled = false;
        var getValueCalled1 = false;
        var getValueCalled2 = false;
        var value = cache.GetOrAdd(1, () =>
        {
            getValueCalled = true;
            return testValue;
        }, CacheExpirationPolicy.AfterSlidingTimeSpan(TimeSpan.FromMilliseconds(1500)));

        Assert.Equal(testValue, value);

        await Task.Delay(1000);

        var secondValue = cache.GetOrAdd(1, () =>
        {
            getValueCalled1 = true;
            return testValue;
        }, CacheExpirationPolicy.AfterSlidingTimeSpan(TimeSpan.FromMilliseconds(1500)));

        await Task.Delay(2000);

        var thirdValue = cache.GetOrAdd(1, () =>
        {
            getValueCalled2 = true;
            return testValue;
        }, CacheExpirationPolicy.AfterSlidingTimeSpan(TimeSpan.FromMilliseconds(1500)));

        Assert.Equal(testValue, secondValue);
        Assert.Equal(testValue, thirdValue);
        Assert.True(getValueCalled);
        Assert.False(getValueCalled1);
        Assert.True(getValueCalled2);
    }

    public static async Task
        GetOrAdd_TwoTimesCalledWithNoTimeSpanExpire_ResultAlwaysTheSameAndGetValueFuncCalledOneTime(
            ICache<int, string> cache)
    {
        const string testValue = "Test1";

        var getValueCalled = false;
        var getValueCalled1 = false;
        var getValueCalled2 = false;
        var value = cache.GetOrAdd(1, () =>
        {
            getValueCalled = true;
            return testValue;
        }, CacheExpirationPolicy.AfterSlidingTimeSpan(TimeSpan.FromMilliseconds(2000)));

        Assert.Equal(testValue, value);

        await Task.Delay(1500);

        var secondValue = cache.GetOrAdd(1, () =>
        {
            getValueCalled1 = true;
            return testValue;
        }, CacheExpirationPolicy.AfterSlidingTimeSpan(TimeSpan.FromMilliseconds(2000)));

        await Task.Delay(1500);

        var thirdValue = cache.GetOrAdd(1, () =>
        {
            getValueCalled2 = true;
            return testValue;
        }, CacheExpirationPolicy.AfterSlidingTimeSpan(TimeSpan.FromMilliseconds(1500)));

        Assert.Equal(testValue, secondValue);
        Assert.Equal(testValue, thirdValue);
        Assert.True(getValueCalled);
        Assert.False(getValueCalled1);
        Assert.False(getValueCalled2);
    }

    public static async Task
        GetOrAddAsync_TwoTimesCalledWithNeverExpire_ResultAlwaysTheSameAndGetValueFuncCalledOneTime(
            ICache<int, string> cache)
    {
        const string testValue = "Test1";

        var getValueCalledCount = 0;
        var value = await cache.GetOrAddAsync(1, () =>
        {
            getValueCalledCount++;
            return testValue;
        }, CacheExpirationPolicy.NeverExpire);

        Assert.Equal(testValue, value);

        var secondValue = await cache.GetOrAddAsync(1, () =>
        {
            getValueCalledCount++;
            return testValue;
        }, CacheExpirationPolicy.NeverExpire);

        Assert.Equal(testValue, secondValue);
        Assert.Equal(1, getValueCalledCount);
    }

    public static async Task
        GetOrAddAsync_TwoTimesCalledWithDateTimeExpire_ResultAlwaysTheSameAndGetValueFuncCalledTwoTime(
            ICache<int, string> cache)
    {
        const string testValue = "Test1";

        var getValueCalledCount = 0;
        var value = await cache.GetOrAddAsync(1, () =>
            {
                getValueCalledCount++;
                return testValue;
            }, CacheExpirationPolicy.AfterAbsoluteDateTime(DateTime.Now.AddMilliseconds(50)))
            ;

        Assert.Equal(testValue, value);

        await Task.Delay(100);

        var secondValue = await cache.GetOrAddAsync(1, () =>
        {
            getValueCalledCount++;
            return testValue;
        }, CacheExpirationPolicy.NeverExpire);

        Assert.Equal(testValue, secondValue);
        Assert.Equal(2, getValueCalledCount);
    }

    public static async Task
        GetOrAddAsync_TwoTimesCalledWithNoDateTimeExpire_ResultAlwaysTheSameAndGetValueFuncCalledOneTime(
            ICache<int, string> cache)
    {
        const string testValue = "Test1";

        var getValueCalled = false;
        var getValueCalled1 = false;
        var getValueCalled2 = false;
        var value = await cache.GetOrAddAsync(1, () =>
            {
                getValueCalled = true;
                return testValue;
            }, CacheExpirationPolicy.AfterAbsoluteDateTime(DateTime.Now.AddMilliseconds(400)))
            ;

        Assert.Equal(testValue, value);

        await Task.Delay(50);

        var secondValue = await cache.GetOrAddAsync(1, () =>
            {
                getValueCalled1 = true;
                return testValue;
            }, CacheExpirationPolicy.AfterAbsoluteDateTime(DateTime.Now.AddMilliseconds(400)))
            ;

        await Task.Delay(400);

        var thirdValue = await cache.GetOrAddAsync(1, () =>
            {
                getValueCalled2 = true;
                return testValue;
            }, CacheExpirationPolicy.AfterAbsoluteDateTime(DateTime.Now.AddMilliseconds(200)))
            ;

        Assert.Equal(testValue, secondValue);
        Assert.Equal(testValue, thirdValue);
        Assert.True(getValueCalled);
        Assert.False(getValueCalled1);
        Assert.True(getValueCalled2);
    }
}
