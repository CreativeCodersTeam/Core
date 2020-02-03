using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CreativeCoders.Core.Enums;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Enums
{
    public class EnumUtilsTests
    {
        [Fact]
        public void GetEnumValues_ForTestEnum_ReturnArrayWithAllValuesFromTestEnum()
        {
            var enumValues = EnumUtils.GetEnumValues<TestEnum>();

            Assert.Equal(4, enumValues.Length);

            var expectedValues = new Enum[] {TestEnum.None, TestEnum.FirstEntry, TestEnum.Extra, TestEnum.SomeMore};
            Assert.Equal(expectedValues.AsEnumerable(), enumValues.AsEnumerable());
        }

        [Fact]
        public void GetFieldInfoForEnum_ForTestEnumNone_ReturnsCorrectFieldInfo()
        {
            var fieldInfo = EnumUtils.GetFieldInfoForEnum(TestEnum.None);

            var fieldInfo2 = typeof(TestEnum).GetField("None");

            Assert.Equal(fieldInfo2, fieldInfo);
        }

        [Fact]
        public void GetValuesFieldInfos_ForTestEnumValues_ReturnsCorrectFieldInfos()
        {
            var fieldInfos = EnumUtils.GetValuesFieldInfos<TestEnum>();

            var fieldInfos2 = TestEnum.None.GetType().GetFields().Where(field => field.FieldType == typeof(TestEnum));

            Assert.Equal(fieldInfos2.AsEnumerable(), fieldInfos.AsEnumerable());
        }

        [Fact]
        public void GetEnumFieldInfos_ForTestEnum_ReturnsCorrectFieldInfos()
        {
            var enumFieldInfos = EnumUtils.GetEnumFieldInfos<TestEnum>();
            var expectedFieldInfos = new Dictionary<Enum, FieldInfo>
            {
                {TestEnum.None, typeof(TestEnum).GetField("None") },
                {TestEnum.FirstEntry, typeof(TestEnum).GetField("FirstEntry") },
                {TestEnum.Extra, typeof(TestEnum).GetField("Extra") },
                {TestEnum.SomeMore, typeof(TestEnum).GetField("SomeMore") }
            };

            Assert.Equal(expectedFieldInfos.Count, enumFieldInfos.Count);

            foreach (var (key, fieldInfo) in expectedFieldInfos)
            {
                var value = enumFieldInfos[key];
                Assert.Equal(fieldInfo, value);
            }
        }

        [Theory]
        [InlineData(0, TestEnumWithInt.None)]
        [InlineData(1, TestEnumWithInt.Ok)]
        [InlineData(2, TestEnumWithInt.Test)]
        [InlineData(3, TestEnumWithInt.None)]
        public void GetEnum_ForTestEnumWithInt_EnumIsCorrect(int intValue, TestEnumWithInt enumValue)
        {
            var convertedValue = EnumUtils.GetEnum<TestEnumWithInt>(intValue);

            Assert.Equal(enumValue, convertedValue);
        }
    }
}