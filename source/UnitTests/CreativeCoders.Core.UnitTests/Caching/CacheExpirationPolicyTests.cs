using CreativeCoders.Core.Caching;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Caching
{
    public class CacheExpirationPolicyTests
    {
        [Fact]
        public void CheckIsExpired_FuncReturnsFalse_ReturnsFalse()
        {
            var expirationPolicy = new CacheExpirationPolicy(() => false);
            
            Assert.False(expirationPolicy.CheckIsExpired());
        }
        
        [Fact]
        public void CheckIsExpired_FuncReturnsTrue_ReturnsTrue()
        {
            var expirationPolicy = new CacheExpirationPolicy(() => true);
            
            Assert.True(expirationPolicy.CheckIsExpired());
        }
        
        // [Fact]
        // public void 
    }
}