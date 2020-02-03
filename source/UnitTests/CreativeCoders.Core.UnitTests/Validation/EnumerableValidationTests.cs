using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CreativeCoders.Validation;
using CreativeCoders.Validation.Rules;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Validation
{
    public class EnumerableValidationTests
    {
        [Fact]
        public void IsEmpty_EmptyList_Valid()
        {
            Assert.True(IsValid(rb => rb.IsEmpty(), new string[0]));
            Assert.True(IsValidList(rb => rb.IsEmpty(), new string[0]));
        }

        [Theory]
        [InlineData("Test")]
        [InlineData("Test", "1234")]
        public void IsEmpty_NotEmptyList_Invalid(params string[] items)
        {
            Assert.False(IsValid(rb => rb.IsEmpty(), items));
            Assert.False(IsValidList(rb => rb.IsEmpty(), items));
        }

        [Fact]
        public void IsNotEmpty_EmptyList_Invalid()
        {
            Assert.False(IsValid(rb => rb.IsNotEmpty(), new string[0]));
            Assert.False(IsValidList(rb => rb.IsNotEmpty(), new string[0]));
        }

        [Theory]
        [InlineData("Test")]
        [InlineData("Test", "1234")]
        public void IsNotEmpty_NotEmptyList_Valid(params string[] items)
        {
            Assert.True(IsValid(rb => rb.IsNotEmpty(), items));
            Assert.True(IsValidList(rb => rb.IsNotEmpty(), items));
        }

        [Theory]
        [InlineData("Test", "1234", "Test", "Hello")]
        [InlineData("1234", "1234", "Test", "Hello")]
        public void Contains_ItemIsInList_Valid(string item, params string[] items)
        {
            Assert.True(IsValid(rb => rb.Contains(item), items));
            Assert.True(IsValidList(rb => rb.Contains(item), items));
        }

        [Theory]
        [InlineData("Test1", "1234", "Test", "Hello")]
        [InlineData("12345", "1234", "Test", "Hello")]
        public void Contains_ItemIsNotInList_Invalid(string item, params string[] items)
        {
            Assert.False(IsValid(rb => rb.Contains(item), items));
            Assert.False(IsValidList(rb => rb.Contains(item), items));
        }

        [Theory]
        [InlineData("Test", "1234", "Test", "Hello")]
        [InlineData("1234", "1234", "Test", "Hello")]
        public void Contains_ListItemIsInList_Valid(string item, params string[] items)
        {
            Assert.True(IsValidList(rb => rb.Contains(item), items));
            Assert.True(IsValid(rb => rb.Contains(item), items));
        }

        [Theory]
        [InlineData("Test1", "1234", "Test", "Hello")]
        [InlineData("12345", "1234", "Test", "Hello")]
        public void Contains_ListItemIsNotInList_Invalid(string item, params string[] items)
        {
            Assert.False(IsValidList(rb => rb.Contains(item), items));
            Assert.False(IsValid(rb => rb.Contains(item), items));
        }

        [Theory]
        [InlineData("Test", "1234", "Test", "Hello")]
        [InlineData("1234", "1234", "Test", "Hello")]
        public void Contains_ItemIsInObjList_Valid(string item, params string[] items)
        { 
            object obj = item;
            var objects = new TestEnumerableImpl();
            items.ForEach(x => objects.Items.Add(x));
            Assert.True(IsValidObjects(rb => rb.Contains(obj), objects));
        }

        [Theory]
        [InlineData("Test1", "1234", "Test", "Hello")]
        [InlineData("12345", "1234", "Test", "Hello")]
        public void Contains_ItemIsNotInObjList_Invalid(string item, params string[] items)
        {
            object obj = item;
            var objects = new TestEnumerableImpl();
            items.ForEach(x => objects.Items.Add(x));
            Assert.False(IsValidObjects(rb => rb.Contains(obj), objects));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1, "Test")]
        [InlineData(2, "1234", "Hello")]
        [InlineData(3, "Text", "Hello", "World")]
        public void Count_ListCountIsCorrect_Valid(int count, params string[] items)
        {
            Assert.True(IsValidList(rb => rb.Count(count), items));
            Assert.True(IsValid(rb => rb.Count(count), items));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2, "Test")]
        [InlineData(1, "1234", "Hello")]
        [InlineData(4, "Text", "Hello", "World")]
        public void Count_ListCountIsIncorrect_Invalid(int count, params string[] items)
        {
            Assert.False(IsValidList(rb => rb.Count(count), items));
            Assert.False(IsValid(rb => rb.Count(count), items));
        }

        [Theory]
        [InlineData("Test", "Test")]
        [InlineData("Hello", "1234", "Hello")]
        [InlineData("World", "Text", "Hello", "World")]
        public void Any_List_Valid(string text, params string[] items)
        {
            Assert.True(IsValidList(rb => rb.Any(x => x.Contains(text)), items));
            Assert.True(IsValid(rb => rb.Any(x => x.Contains(text)), items));
        }

        [Theory]
        [InlineData("Test1", "Test")]
        [InlineData("Hello1", "1234", "Hello")]
        [InlineData("World1", "Text", "Hello", "World")]
        public void Any_List_Invalid(string text, params string[] items)
        {
            Assert.False(IsValidList(rb => rb.Any(x => x.Contains(text)), items));
            Assert.False(IsValid(rb => rb.Any(x => x.Contains(text)), items));
        }

        [Theory]
        [InlineData(4, "Test")]
        [InlineData(5, "12345", "Hello")]
        [InlineData(5, "Text1", "Hello", "World")]
        public void All_List_Valid(int length, params string[] items)
        {
            Assert.True(IsValidList(rb => rb.All(x => x.Length == length), items));
            Assert.True(IsValid(rb => rb.All(x => x.Length == length), items));
        }

        [Theory]
        [InlineData(4, "Test1")]
        [InlineData(4, "12345", "Hello")]
        [InlineData(5, "Text", "Hello1", "World23")]
        public void All_List_Invalid(int length, params string[] items)
        {
            Assert.False(IsValidList(rb => rb.All(x => x.Length == length), items));
            Assert.False(IsValid(rb => rb.All(x => x.Length == length), items));
        }

        [Theory]
        [InlineData("Test")]
        [InlineData("12345")]
        [InlineData("Text1")]
        public void Single_List_Valid(params string[] items)
        {
            Assert.True(IsValidList(rb => rb.Single(), items));
            Assert.True(IsValid(rb => rb.Single(), items));
        }

        [Theory]
        [InlineData("12345", "Hello")]
        [InlineData("Text1", "World", "Hello")]
        public void Single_List_Invalid(params string[] items)
        {
            Assert.False(IsValidList(rb => rb.Single(), items));
            Assert.False(IsValid(rb => rb.Single(), items));
        }
        
        [Fact]
        public void Single_EmptyList_Invalid()
        {
            var items = new string[0];
            
            Assert.False(IsValidList(rb => rb.Single(), items));
            Assert.False(IsValid(rb => rb.Single(), items));
        }

        private static bool IsValid(Action<IPropertyRuleBuilder<TestDataObject, IEnumerable<string>>> setupRule, IEnumerable<string> strItems)
        {
            var validator = new TestDataObjectValidator(v => setupRule(v.RuleFor(x => x.StrItems)));

            var testData = new TestDataObject { StrItems = strItems };

            var validationResult = validator.Validate(testData);

            return validationResult.IsValid;
        }

        private static bool IsValidObjects(Action<IPropertyRuleBuilder<TestDataObject, IEnumerable>> setupRule, IEnumerable items)
        {
            var validator = new TestDataObjectValidator(v => setupRule(v.RuleFor(x => x.ObjItems)));

            var testData = new TestDataObject { ObjItems = items };

            var validationResult = validator.Validate(testData);

            return validationResult.IsValid;
        }

        private static bool IsValidList(Action<IPropertyRuleBuilder<TestDataObject, IList<string>>> setupRule, IEnumerable<string> strItems)
        {
            var validator = new TestDataObjectValidator(v => setupRule(v.RuleFor(x => x.StrList)));

            var testData = new TestDataObject { StrList = strItems.ToList() };

            var validationResult = validator.Validate(testData);

            return validationResult.IsValid;
        }
    }
}