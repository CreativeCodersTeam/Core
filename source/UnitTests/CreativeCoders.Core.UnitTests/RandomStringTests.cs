using Xunit;

namespace CreativeCoders.Core.UnitTests
{
    public class RandomStringTests
    {
        [Fact]
        public void RandomStringNewTest()
        {
            var s = RandomString.Create();

            Assert.True(!string.IsNullOrEmpty(s));
        }

        [Fact]
        public void RandomStringNewBufferSizeTest()
        {
            var s = RandomString.Create(10);

            Assert.True(!string.IsNullOrEmpty(s));
        }
    }
}
