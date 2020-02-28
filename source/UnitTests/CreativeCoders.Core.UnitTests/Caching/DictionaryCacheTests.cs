using System.Threading.Tasks;
using CreativeCoders.Core.Caching;
using CreativeCoders.Core.Caching.Default;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Caching
{
    public class DictionaryCacheTests
    {
        [Fact]
        public void TryGet_KeyNotExists_ReturnFalse()
        {
            var cache = new DictionaryCache<int, string>();

            var keyExists = cache.TryGet(1, out _);

            Assert.False(keyExists);
        }
        
        [Fact]
        public async Task TryGetAsync_KeyNotExists_ReturnFalse()
        {
            var cache = new DictionaryCache<int, string>();

            var cacheRequestResult = await cache.TryGetAsync(1);
            
            Assert.False(cacheRequestResult.EntryExists);
        }

        [Fact]
        public void TryGet_KeyExits_ReturnsTrueAndValue()
        {
            var cache = new DictionaryCache<int, string>();
            cache.AddOrUpdate(1, "TestValue");

            var keyExists = cache.TryGet(1, out var value);

            Assert.True(keyExists);
            Assert.Equal("TestValue", value);
        }
        
        [Fact]
        public async Task TryGetAsync_KeyExits_ReturnsTrueAndValue()
        {
            var cache = new DictionaryCache<int, string>();
            await cache.AddOrUpdateAsync(1, "TestValue");

            var cacheRequestResult = await cache.TryGetAsync(1);

            Assert.True(cacheRequestResult.EntryExists);
            Assert.Equal("TestValue", cacheRequestResult.Value);
        }

        [Fact]
        public void TryGet_AddOrUpdateTwoValue_ReturnsLastValue()
        {
            var cache = new DictionaryCache<int, string>();
            cache.AddOrUpdate(1, "TestValue");
            cache.AddOrUpdate(1, "TestValue1234");

            var keyExists = cache.TryGet(1, out var value);

            Assert.True(keyExists);
            Assert.Equal("TestValue1234", value);
        }
        
        [Fact]
        public async Task TryGetAsync_AddOrUpdateTwoValue_ReturnsLastValue()
        {
            var cache = new DictionaryCache<int, string>();
            await cache.AddOrUpdateAsync(1, "TestValue");
            await cache.AddOrUpdateAsync(1, "TestValue1234");

            var cacheRequestResult = await cache.TryGetAsync(1);

            Assert.True(cacheRequestResult.EntryExists);
            Assert.Equal("TestValue1234", cacheRequestResult.Value);
        }

        [Fact]
        public void Clear_TryGetValue_ReturnFalse()
        {
            var cache = new DictionaryCache<int, string>();
            cache.AddOrUpdate(1, "TestValue1");
            cache.AddOrUpdate(2, "TestValue2");
            cache.Clear();

            var keyExists1 = cache.TryGet(1, out _);
            var keyExists2 = cache.TryGet(2, out _);

            Assert.False(keyExists1);
            Assert.False(keyExists2);
        }
        
        [Fact]
        public async Task ClearAsync_TryGetValue_ReturnFalse()
        {
            var cache = new DictionaryCache<int, string>();
            await cache.AddOrUpdateAsync(1, "TestValue1");
            await cache.AddOrUpdateAsync(2, "TestValue2");
            await cache.ClearAsync();

            var cacheRequestResult1 = await cache.TryGetAsync(1);
            var cacheRequestResult2 = await cache.TryGetAsync(2);

            Assert.False(cacheRequestResult1.EntryExists);
            Assert.False(cacheRequestResult2.EntryExists);
        }
        
        [Fact]
        public void GetEntry_ExistingKey_ReturnEntry()
        {
            var cache = new DictionaryCache<int, string>();
            cache.AddOrUpdate(1, "TestValue1");
            cache.AddOrUpdate(2, "TestValue2");

            var cacheEntry = cache.GetEntry(1);

            Assert.Equal(1, cacheEntry.Key);
            Assert.Equal("TestValue1", cacheEntry.Value);
            Assert.Same(CacheExpirationPolicyTests.NeverExpire, cacheEntry.ExpirationPolicy);
        }
        
        [Fact]
        public async Task GetEntryAsync_ExistingKey_ReturnEntry()
        {
            var cache = new DictionaryCache<int, string>();
            await cache.AddOrUpdateAsync(1, "TestValue1");
            await cache.AddOrUpdateAsync(2, "TestValue2");
            

            var cacheEntry = await cache.GetEntryAsync(1);

            Assert.Equal(1, cacheEntry.Key);
            Assert.Equal("TestValue1", cacheEntry.Value);
            Assert.Same(CacheExpirationPolicyTests.NeverExpire, cacheEntry.ExpirationPolicy);
        }
        
        [Fact]
        public void Remove_TryGetValue_ReturnFalse()
        {
            var cache = new DictionaryCache<int, string>();
            cache.AddOrUpdate(1, "TestValue1");
            cache.AddOrUpdate(2, "TestValue2");
            cache.Remove(1);

            var keyExists1 = cache.TryGet(1, out _);
            var keyExists2 = cache.TryGet(2, out _);

            Assert.False(keyExists1);
            Assert.True(keyExists2);
        }
        
        [Fact]
        public async Task RemoveAsync_TryGetValue_ReturnFalse()
        {
            var cache = new DictionaryCache<int, string>();
            await cache.AddOrUpdateAsync(1, "TestValue");
            await cache.AddOrUpdateAsync(2, "TestValue2");
            await cache.RemoveAsync(1);

            var cacheRequestResult1 = await cache.TryGetAsync(1);
            var cacheRequestResult2 = await cache.TryGetAsync(2);

            Assert.False(cacheRequestResult1.EntryExists);
            Assert.True(cacheRequestResult2.EntryExists);
        }

        [Fact]
        public void GetValue_GetExisting_ReturnsValue()
        {
            var cache = new DictionaryCache<int, string>();
            cache.AddOrUpdate(1, "TestValue");

            var value = cache.GetValue(1);

            Assert.Equal("TestValue", value);
        }
        
        [Fact]
        public async Task GetValueAsync_GetExisting_ReturnsValue()
        {
            var cache = new DictionaryCache<int, string>();
            await cache.AddOrUpdateAsync(1, "TestValue");

            var value = await cache.GetValueAsync(1);

            Assert.Equal("TestValue", value);
        }

        [Fact]
        public void GetValue_KeyNotExists_ThrowsException()
        {
            var cache = new DictionaryCache<int, string>();

            var exception = Assert.Throws<CacheEntryNotFoundException>(() => cache.GetValue(1));
            
            Assert.Equal("1", exception.Key);
        }
        
        [Fact]
        public async Task GetValueAsync_KeyNotExists_ThrowsException()
        {
            var cache = new DictionaryCache<int, string>();

            var exception =
                await Assert.ThrowsAsync<CacheEntryNotFoundException>(async () => await cache.GetValueAsync(1));
            
            Assert.Equal("1", exception.Key);
        }

        [Fact]
        public void GetValue_KeyNotExists_ReturnNull()
        {
            var cache = new DictionaryCache<int, string>();

            var value = cache.GetValue(1, false);

            Assert.Null(value);
        }
        
        [Fact]
        public async Task GetValueAsync_KeyNotExists_ReturnNull()
        {
            var cache = new DictionaryCache<int, string>();

            var value = await cache.GetValueAsync(1, false);

            Assert.Null(value);
        }

        [Fact]
        public void GetValue_KeyNotExists_ReturnDefaultValue()
        {
            var cache = new DictionaryCache<int, string>();

            var value = cache.GetValueOrDefault(1, "DefaultValue");

            Assert.Equal("DefaultValue", value);
        }
        
        [Fact]
        public async Task GetValueAsync_KeyNotExists_ReturnDefaultValue()
        {
            var cache = new DictionaryCache<int, string>();

            var value = await cache.GetValueOrDefaultAsync(1, "DefaultValue");

            Assert.Equal("DefaultValue", value);
        }

        [Fact]
        public void GetValue_KeyNotExists_ReturnNewValueAndStoreToCache()
        {
            var cache = new DictionaryCache<int, string>();

            var value = cache.GetOrAdd(1, () => "DefaultValue");
            var secondValue = cache.GetValue(1);

            Assert.Equal("DefaultValue", value);
            Assert.Equal("DefaultValue", secondValue);
        }
        
        [Fact]
        public async Task GetValueAsync_KeyNotExists_ReturnNewValueAndStoreToCache()
        {
            var cache = new DictionaryCache<int, string>();

            var value = await cache.GetOrAddAsync(1, () => "DefaultValue");
            var secondValue = await cache.GetValueAsync(1);

            Assert.Equal("DefaultValue", value);
            Assert.Equal("DefaultValue", secondValue);
        }

        [Fact]
        public void GetValue_KeyExists_ReturnOldValue()
        {
            var cache = new DictionaryCache<int, string>();
            cache.AddOrUpdate(1, "TestValue");

            var value = cache.GetOrAdd(1, () => "DefaultValue");
            var secondValue = cache.GetValue(1);

            Assert.Equal("TestValue", value);
            Assert.Equal("TestValue", secondValue);
        }
        
        [Fact]
        public async Task GetValueAsync_KeyExists_ReturnOldValue()
        {
            var cache = new DictionaryCache<int, string>();
            await cache.AddOrUpdateAsync(1, "TestValue");

            var value = await cache.GetOrAddAsync(1, () => "DefaultValue");
            var secondValue = await cache.GetValueAsync(1);

            Assert.Equal("TestValue", value);
            Assert.Equal("TestValue", secondValue);
        }

        [Fact]
        public void TryGet_AfterExpiration_ReturnsFalse()
        {
            var expirationPolicy = A.Fake<ICacheExpirationPolicy>();
            A.CallTo(() => expirationPolicy.CheckIsExpired()).Returns(false);
            
            var cache = new DictionaryCache<int, string>();
            cache.AddOrUpdate(1, "TestValue", expirationPolicy);

            var value = cache.GetValue(1);
            
            Assert.Equal("TestValue", value);

            A.CallTo(() => expirationPolicy.CheckIsExpired()).Returns(true);
            
            Assert.False(cache.TryGet(1, out _));
        }
        
        [Fact]
        public async Task TryGetAsync_AfterExpiration_ReturnsFalse()
        {
            var expirationPolicy = A.Fake<ICacheExpirationPolicy>();
            A.CallTo(() => expirationPolicy.CheckIsExpired()).Returns(false);
            
            var cache = new DictionaryCache<int, string>();
            await cache.AddOrUpdateAsync(1, "TestValue", expirationPolicy);

            var value = await cache.GetValueAsync(1);
            
            Assert.Equal("TestValue", value);

            A.CallTo(() => expirationPolicy.CheckIsExpired()).Returns(true);
            
            Assert.False(cache.TryGet(1, out _));
        }
    }
}