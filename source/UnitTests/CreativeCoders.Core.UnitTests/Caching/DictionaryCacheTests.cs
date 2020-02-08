using CreativeCoders.Core.Caching;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Caching
{
    public class DictionaryCacheTests
    {
        [Fact]
        public void Ctor_Called_NoException()
        {
            var cache = new DictionaryCache<int, string>();
            Assert.NotNull(cache);
        }

        [Fact]
        public void TryGetValue_KeyNotExists_ReturnFalse()
        {
            var cache = new DictionaryCache<int, string>();

            var keyExists = cache.TryGetValue(1, out _);

            Assert.False(keyExists);
        }

        [Fact]
        public void TryGetValue_KeyExits_ReturnsTrueAndValue()
        {
            var cache = new DictionaryCache<int, string>();
            cache.AddOrUpdate(1, "TestValue");

            var keyExists = cache.TryGetValue(1, out var value);

            Assert.True(keyExists);
            Assert.Equal("TestValue", value);
        }

        [Fact]
        public void TryGetValue_AddOrUpdateTwoValue_ReturnsLastValue()
        {
            var cache = new DictionaryCache<int, string>();
            cache.AddOrUpdate(1, "TestValue");
            cache.AddOrUpdate(1, "TestValue1234");

            var keyExists = cache.TryGetValue(1, out var value);

            Assert.True(keyExists);
            Assert.Equal("TestValue1234", value);
        }

        [Fact]
        public void Clear_TryGetValue_ReturnFalse()
        {
            var cache = new DictionaryCache<int, string>();
            cache.AddOrUpdate(1, "TestValue");
            cache.Clear();

            var keyExists = cache.TryGetValue(1, out _);

            Assert.False(keyExists);
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
        public void GetValue_KeyNotExists_ThrowsException()
        {
            var cache = new DictionaryCache<int, string>();
            
            var key = Assert.Throws<CacheEntryNotFoundException>(() => cache.GetValue(1)).Key;
            Assert.Equal("1", key);
        }

        [Fact]
        public void GetValue_KeyNotExists_ReturnNull()
        {
            var cache = new DictionaryCache<int, string>();

            var value = cache.GetValue(1, false);

            Assert.Null(value);
        }

        [Fact]
        public void GetValue_KeyNotExists_ReturnDefaultValue()
        {
            var cache = new DictionaryCache<int, string>();

            var value = cache.GetValue(1, "DefaultValue");

            Assert.Equal("DefaultValue", value);
        }

        [Fact]
        public void GetValue_KeyNotExists_ReturnNewValueAndStoreToCache()
        {
            var cache = new DictionaryCache<int, string>();

            var value = cache.GetValue(1, () => "DefaultValue");
            var secondValue = cache.GetValue(1);

            Assert.Equal("DefaultValue", value);
            Assert.Equal("DefaultValue", secondValue);
        }

        [Fact]
        public void GetValue_KeyExists_ReturnOldValue()
        {
            var cache = new DictionaryCache<int, string>();
            cache.AddOrUpdate(1, "TestValue");

            var value = cache.GetValue(1, () => "DefaultValue");
            var secondValue = cache.GetValue(1);

            Assert.Equal("TestValue", value);
            Assert.Equal("TestValue", secondValue);
        }
    }
}