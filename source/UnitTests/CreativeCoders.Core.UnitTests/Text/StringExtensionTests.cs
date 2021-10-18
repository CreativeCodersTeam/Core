using System.Security;
using System.Text;
using CreativeCoders.Core.SysEnvironment;
using CreativeCoders.Core.Text;
using FluentAssertions;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Text
{
    public class StringExtensionTests
    {
        [Fact]
        public void ToSecureStringMakeReadOnlyTest()
        {
            const string s = "testText";
            var secureString = s.ToSecureString(true);
            Assert.True(secureString.IsReadOnly());
        }

        [Fact]
        public void ToSecureStringTest()
        {
            const string s = "testText";
            var secureString = s.ToSecureString();
            Assert.True(!secureString.IsReadOnly());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void IsNullOrEmptyTestTrue(string value)
        {
            Assert.True(value.IsNullOrEmpty());            
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void IsNotNullOrEmptyTestFalse(string value)
        {
            Assert.False(value.IsNotNullOrEmpty());
        }

        [Theory]
        [InlineData("hello")]
        [InlineData("world")]
        public void IsNullOrEmptyTestFalse(string value)
        {
            Assert.False(value.IsNullOrEmpty());
        }

        [Theory]
        [InlineData("hello")]
        [InlineData("world")]
        public void IsNotNullOrEmptyTestTrue(string value)
        {
            Assert.True(value.IsNotNullOrEmpty());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        public void IsNullOrWhiteSpaceTestTrue(string value)
        {
            Assert.True(value.IsNullOrWhiteSpace());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        public void IsNotNullOrWhiteSpaceTestFalse(string value)
        {
            Assert.False(value.IsNotNullOrWhiteSpace());
        }

        [Theory]
        [InlineData("hello")]
        [InlineData(" world ")]
        public void IsNullOrWhiteSpaceTestFalse(string value)
        {
            Assert.False(value.IsNullOrWhiteSpace());
        }

        [Theory]
        [InlineData("hello")]
        [InlineData(" world ")]
        public void IsNotNullOrWhiteSpaceTestTrue(string value)
        {
            Assert.True(value.IsNotNullOrWhiteSpace());
        }

        [Fact]
        public void AppendLine_AppendText_ReturnsAppendedText()
        {
            var sb = new StringBuilder();

            sb.AppendLine("Test", false);

            Assert.Equal("Test" + Env.NewLine, sb.ToString());
        }

        [Fact]
        public void AppendLine_AppendTextWithSuppression_ReturnsEmptyString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("Test", true);

            Assert.Equal(string.Empty, sb.ToString());
        }

        [Fact]
        public void AppendLine_AppendTextToExistingText_ReturnsTextWithAppendedText()
        {
            var sb = new StringBuilder();

            sb.AppendLine("1234");
            sb.AppendLine("Test", false);

            Assert.Equal("1234"+ Env.NewLine + "Test" + Env.NewLine, sb.ToString());
        }

        [Fact]
        public void AppendLine_AppendTextWithSuppressionToExistingText_ReturnsExistingText()
        {
            var sb = new StringBuilder();

            sb.AppendLine("1234");
            sb.AppendLine("Test", true);

            Assert.Equal("1234" + Env.NewLine, sb.ToString());
        }

        [Theory]
        [InlineData("Hello World", new[]{'o', 'W'}, "Hell rld")]
        [InlineData(@"some\:_file?*.txt", new[] { '\\', ':', '?', '*' }, "some_file.txt")]
        public void Filter_(string input, char[] filteredChars, string expected)
        {
            var filteredText = input.Filter(filteredChars);

            Assert.Equal(expected, filteredText);
        }

        [Fact]
        public void ToNormalString_TestText_ReturnsEncryptedText()
        {
            const string expectedText = "TestText";

            var secureString = expectedText.ToSecureString();
            
            Assert.Equal(expectedText, secureString.ToNormalString());
        }

        [Fact]
        public void ToNormalString_EmptySecureString_ReturnsEmptyString()
        {
            var secureString = new SecureString();

            Assert.Equal(string.Empty, secureString.ToNormalString());
        }

        [Fact]
        public void ToNormalString_NullSecureString_ReturnsEmptyString()
        {
            SecureString secureString = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.Equal(string.Empty, secureString.ToNormalString());
        }

        [Fact]
        public void AppendLineIf_AppendToTrue_LineIsAppended()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Test");

            // Act
            sb.AppendLineIf(true, "1234");

            // Assert
            sb.ToString()
                .Should()
                .Be($"Test{Env.NewLine}1234{Env.NewLine}");
        }

        [Fact]
        public void AppendLineIf_AppendToFalse_LineIsNotAppended()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Test");

            // Act
            sb.AppendLineIf(false, "1234");

            // Assert
            sb.ToString()
                .Should()
                .Be($"Test{Env.NewLine}");
        }

        [Fact]
        public void AppendIf_AppendToTrue_TextIsAppended()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Test");

            // Act
            sb.AppendIf(true, "1234");

            // Assert
            sb.ToString()
                .Should()
                .Be($"Test{Env.NewLine}1234");
        }

        [Fact]
        public void AppendIf_AppendToFalse_TextIsNotAppended()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Test");

            // Act
            sb.AppendLineIf(false, "1234");

            // Assert
            sb.ToString()
                .Should()
                .Be($"Test{Env.NewLine}");
        }
    }
}
