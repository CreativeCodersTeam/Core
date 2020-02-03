using Xunit;

namespace CreativeCoders.Core.UnitTests
{
    public class RandomStringTests
    {
        [Fact]
        public void RandomStringNewTest()
        {
            var s = RandomString.New();

            Assert.True(!string.IsNullOrEmpty(s));
        }

        [Fact]
        public void RandomStringNewBufferSizeTest()
        {
            var s = RandomString.New(10);

            Assert.True(!string.IsNullOrEmpty(s));
        }
    }
}
