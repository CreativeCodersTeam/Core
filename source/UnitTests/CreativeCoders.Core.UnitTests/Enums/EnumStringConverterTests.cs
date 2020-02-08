using System;
using CreativeCoders.Core.Enums;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Enums
{
    public class EnumStringConverterTests
    {
        [Fact]
        public void Convert_EnumValueWithoutAttribute_ReturnsEnumFieldName()
        {
            var converter = new EnumStringConverter();

            var text = converter.Convert(ConsoleColor.Gray);

            Assert.Equal("Gray", text);
        }

        [Fact]
        public void Convert_NullEnum_ReturnsStringEmpty()
        {
            var converter = new EnumStringConverter();

            var text = converter.Convert(null);

            Assert.Equal(string.Empty, text);
        }

        [Fact]
        public void Convert_EnumValueWithAttribute_ReturnsAttributeText()
        {
            var converter = new EnumStringConverter();

            var text = converter.Convert(TestEnum.FirstEntry);

            Assert.Equal("first entry", text);
        }

        [Fact]
        public void Convert_TextForEnum_ReturnsEnumValue()
        {
            var converter = new EnumStringConverter();

            var enumValue = converter.Convert<ConsoleColor>("Gray");

            Assert.Equal(ConsoleColor.Gray, enumValue);
        }

        [Fact]
        public void Convert_AttributeTextForEnum_ReturnsEnumValue()
        {
            var converter = new EnumStringConverter();

            var enumValue = converter.Convert<TestEnum>("first entry");

            Assert.Equal(TestEnum.FirstEntry, enumValue);
        }

        [Fact]
        public void Convert_NoneExistentText_ReturnsDefaultEnumValue()
        {
            var converter = new EnumStringConverter();

            var enumValue = converter.Convert<TestEnum>("testing");

            Assert.Equal(TestEnum.None, enumValue);
        }

        [Fact]
        public void Convert_AttributeTextForEnumWithCacheUsage_ReturnsEnumValue()
        {
            var converter = new EnumStringConverter();

            var _ = converter.Convert<TestEnum>("first entry");
            var secondEnumValue = converter.Convert<TestEnum>("first entry");

            Assert.Equal(TestEnum.FirstEntry, secondEnumValue);
        }

        [Fact]
        public void ClearCaches_Call_NoExceptions()
        {
            EnumStringConverter.ClearCaches();
        }
    }
}