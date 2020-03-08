using System.Threading.Tasks;
using CreativeCoders.Core.Caching;
using CreativeCoders.Core.Caching.Default;
using FakeItEasy.Sdk;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Caching
{
    public class DictionaryCacheTests
    {
        [Fact]
        public void TryGet_KeyNotExists_ReturnFalse()
        {
            TestCaching.TryGet_KeyNotExists_ReturnFalse(CreateCache<int, string>());
        }

        [Fact]
        public void TryGet_KeyNotExistsWithRegion_ReturnFalse()
        {
            TestCaching.TryGet_KeyNotExistsWithRegion_ReturnFalse(CreateCache<int, string>());
        }
        
        [Fact]
        public async Task TryGetAsync_KeyNotExists_ReturnFalse()
        {
            await TestCaching.TryGetAsync_KeyNotExists_ReturnFalse(CreateCache<int, string>());
        }

        [Fact]
        public async Task TryGetAsync_KeyNotExistsWithRegion_ReturnFalse()
        {
            await TestCaching.TryGetAsync_KeyNotExistsWithRegion_ReturnFalse(CreateCache<int, string>());
        }

        [Fact]
        public void TryGet_KeyExists_ReturnsTrueAndValue()
        {
            TestCaching.TryGet_KeyExists_ReturnsTrueAndValue(CreateCache<int, string>());
        }

        [Fact]
        public void TryGet_KeyExistsWithRegion_ReturnsTrueAndValue()
        {
            TestCaching.TryGet_KeyExistsWithRegion_ReturnsTrueAndValue(CreateCache<int, string>());
        }
        
        [Fact]
        public async Task TryGetAsync_KeyExists_ReturnsTrueAndValue()
        {
            await TestCaching.TryGetAsync_KeyExists_ReturnsTrueAndValue(CreateCache<int, string>());
        }

        [Fact]
        public async Task TryGetAsync_KeyExistsWithRegion_ReturnsTrueAndValue()
        {
            await TestCaching.TryGetAsync_KeyExistsWithRegion_ReturnsTrueAndValue(CreateCache<int, string>());
        }

        [Fact]
        public void TryGet_AddOrUpdateTwoValue_ReturnsLastValue()
        {
            TestCaching.TryGet_AddOrUpdateTwoValue_ReturnsLastValue(CreateCache<int, string>());
        }

        [Fact]
        public void TryGet_AddOrUpdateTwoValueWithRegions_ReturnsLastValue()
        {
            TestCaching.TryGet_AddOrUpdateTwoValueWithRegions_ReturnsLastValue(CreateCache<int, string>());
        }
        
        [Fact]
        public async Task TryGetAsync_AddOrUpdateTwoValue_ReturnsLastValue()
        {
            await TestCaching.TryGetAsync_AddOrUpdateTwoValue_ReturnsLastValue(CreateCache<int, string>());
        }

        [Fact]
        public async Task TryGetAsync_AddOrUpdateTwoValueWithRegions_ReturnsLastValue()
        {
            await TestCaching.TryGetAsync_AddOrUpdateTwoValueWithRegions_ReturnsLastValue(CreateCache<int, string>());
        }

        [Fact]
        public void Clear_TryGetValue_ReturnFalse()
        {
            TestCaching.Clear_TryGetValue_ReturnFalse(CreateCache<int, string>());
        }

        [Fact]
        public void Clear_TryGetValueWithRegions_ReturnFalse()
        {
            TestCaching.Clear_TryGetValueWithRegions_ReturnFalse(CreateCache<int, string>());
        }
        
        [Fact]
        public async Task ClearAsync_TryGetValue_ReturnFalse()
        {
            await TestCaching.ClearAsync_TryGetValue_ReturnFalse(CreateCache<int, string>());
        }

        [Fact]
        public async Task ClearAsync_TryGetValueWithRegions_ReturnFalse()
        {
            await TestCaching.ClearAsync_TryGetValueWithRegions_ReturnFalse(CreateCache<int, string>());
        }
        
        [Fact]
        public void Remove_TryGetValue_ReturnFalse()
        {
            TestCaching.Remove_TryGetValue_ReturnFalse(CreateCache<int, string>());
        }

        [Fact]
        public void Remove_WithOutRegionsTryGetValue_ReturnFalse()
        {
            TestCaching.Remove_WithOutRegionsTryGetValue_ReturnFalse(CreateCache<int, string>());
        }

        [Fact]
        public void Remove_WithRegionsTryGetValue_ReturnFalse()
        {
            TestCaching.Remove_WithRegionsTryGetValue_ReturnFalse(CreateCache<int, string>());
        }
        
        [Fact]
        public async Task RemoveAsync_TryGetValue_ReturnFalse()
        {
            await TestCaching.RemoveAsync_TryGetValue_ReturnFalse(CreateCache<int, string>());
        }

        [Fact]
        public async Task RemoveAsync_WithOutRegionsTryGetValue_ReturnFalse()
        {
            await TestCaching.RemoveAsync_WithOutRegionsTryGetValue_ReturnFalse(CreateCache<int, string>());
        }

        [Fact]
        public async Task RemoveAsync_WithRegionsTryGetValue_ReturnFalse()
        {
            await TestCaching.RemoveAsync_WithRegionsTryGetValue_ReturnFalse(CreateCache<int, string>());
        }

        [Fact]
        public void GetValue_GetExisting_ReturnsValue()
        {
            TestCaching.GetValue_GetExisting_ReturnsValue(CreateCache<int, string>());
        }

        [Fact]
        public void GetValue_GetExistingWithRegion_ReturnsValue()
        {
            TestCaching.GetValue_GetExistingWithRegion_ReturnsValue(CreateCache<int, string>());
        }
        
        [Fact]
        public async Task GetValueAsync_GetExisting_ReturnsValue()
        {
            await TestCaching.GetValueAsync_GetExisting_ReturnsValue(CreateCache<int, string>());
        }

        [Fact]
        public async Task GetValueAsync_GetExistingWithRegion_ReturnsValue()
        {
            await TestCaching.GetValueAsync_GetExistingWithRegion_ReturnsValue(CreateCache<int, string>());
        }

        [Fact]
        public void GetValue_KeyNotExists_ThrowsException()
        {
            TestCaching.GetValue_KeyNotExists_ThrowsException(CreateCache<int, string>());
        }

        [Fact]
        public void GetValue_KeyNotExistsWithRegions_ThrowsException()
        {
            TestCaching.GetValue_KeyNotExistsWithRegions_ThrowsException(CreateCache<int, string>());
        }
        
        [Fact]
        public async Task GetValueAsync_KeyNotExists_ThrowsException()
        {
            await TestCaching.GetValueAsync_KeyNotExists_ThrowsException(CreateCache<int, string>());
        }

        [Fact]
        public async Task GetValueAsync_KeyNotExistsWithRegions_ThrowsException()
        {
            await TestCaching.GetValueAsync_KeyNotExistsWithRegions_ThrowsException(CreateCache<int, string>());
        }

        [Fact]
        public void GetValue_KeyNotExists_ReturnNull()
        {
            TestCaching.GetValue_KeyNotExists_ReturnNull(CreateCache<int, string>());
        }
        
        [Fact]
        public async Task GetValueAsync_KeyNotExists_ReturnNull()
        {
            await TestCaching.GetValueAsync_KeyNotExists_ReturnNull(CreateCache<int, string>());
        }

        [Fact]
        public void GetValue_KeyNotExists_ReturnDefaultValue()
        {
            TestCaching.GetValue_KeyNotExists_ReturnDefaultValue(CreateCache<int, string>());
        }
        
        [Fact]
        public async Task GetValueAsync_KeyNotExists_ReturnDefaultValue()
        {
            await TestCaching.GetValueAsync_KeyNotExists_ReturnDefaultValue(CreateCache<int, string>());
        }

        [Fact]
        public void GetValue_KeyNotExists_ReturnNewValueAndStoreToCache()
        {
            TestCaching.GetValue_KeyNotExists_ReturnNewValueAndStoreToCache(CreateCache<int, string>());
        }
        
        [Fact]
        public async Task GetValueAsync_KeyNotExists_ReturnNewValueAndStoreToCache()
        {
            await TestCaching.GetValueAsync_KeyNotExists_ReturnNewValueAndStoreToCache(CreateCache<int, string>());
        }

        [Fact]
        public void GetValue_KeyExists_ReturnOldValue()
        {
            TestCaching.GetValue_KeyExists_ReturnOldValue(CreateCache<int, string>());
        }
        
        [Fact]
        public async Task GetValueAsync_KeyExists_ReturnOldValue()
        {
            await TestCaching.GetValueAsync_KeyExists_ReturnOldValue(CreateCache<int, string>());
        }

        [Fact]
        public void TryGet_AfterExpiration_ReturnsFalse()
        {
            TestCaching.TryGet_AfterExpiration_ReturnsFalse(CreateCache<int, string>());
        }
        
        [Fact]
        public async Task TryGetAsync_AfterExpiration_ReturnsFalse()
        {
            await TestCaching.TryGetAsync_AfterExpiration_ReturnsFalse(CreateCache<int, string>());
        }
        
        [Fact]
        public void TryGet_AfterExpirationTimeSpan_ReturnsFalse()
        {
            TestCaching.TryGet_AfterExpirationTimeSpan_ReturnsFalse(CreateCache<int, string>());
        }
        
        [Fact]
        public async Task TryGetAsync_AfterExpirationTimeSpan_ReturnsFalse()
        {
            await TestCaching.TryGetAsync_AfterExpirationTimeSpan_ReturnsFalse(CreateCache<int, string>());
        }

        [Fact]
        public void GetOrAdd_TwoTimesCalled_ResultAlwaysTheSameAndGetValueFuncCalledOneTime()
        {
            TestCaching.GetOrAdd_TwoTimesCalled_ResultAlwaysTheSameAndGetValueFuncCalledOneTime(CreateCache<int, string>());
        }
        
        private static ICache<TKey, TValue> CreateCache<TKey, TValue>()
        {
            return new DictionaryCache<TKey, TValue>();
        }
    }
}