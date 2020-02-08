using Xunit;

namespace CreativeCoders.Core.UnitTests
{
    public class PatternMatcherTests
    {
        [Fact]
        public void PatternMatcherMatchesPatternAsteriskTest()
        {
            Assert.True(PatternMatcher.MatchesPattern("filename.txt", "*.txt"));
            Assert.True(PatternMatcher.MatchesPattern("filename.txt", "filename.*"));
            Assert.True(PatternMatcher.MatchesPattern("filename.txt", "file*.txt"));
            Assert.True(PatternMatcher.MatchesPattern("filename.txt", "file*.*"));
            Assert.True(PatternMatcher.MatchesPattern("filename.txt", "fi*me.txt"));
            Assert.True(PatternMatcher.MatchesPattern("filename.txt", "fi*me.*"));
            Assert.False(PatternMatcher.MatchesPattern("filename.txt", "*.doc"));
            // ReSharper disable once StringLiteralTypo
            Assert.False(PatternMatcher.MatchesPattern("filename.txt", "filex*.txt"));
        }

        [Fact]
        public void PatternMatcherMatchesPatternQuestionMarkTest()
        {
            // ReSharper disable once StringLiteralTypo
            Assert.True(PatternMatcher.MatchesPattern("filename.txt", "filenam?.txt"));
        }
    }
}