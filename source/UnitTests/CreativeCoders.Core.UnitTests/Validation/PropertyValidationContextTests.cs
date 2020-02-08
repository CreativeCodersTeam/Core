using System;
using System.Collections;
using System.Linq.Expressions;
using CreativeCoders.Validation;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Validation
{
    public class PropertyValidationContextTests
    {
        [Fact]
        public void Ctor_CallWithNull_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new PropertyValidationContext<object, object>(null, null));

            Assert.Throws<ArgumentNullException>(() =>
                new PropertyValidationContext<object, object>(null, A.Fake<IValidationContext<object>>()));

            Expression<Func<TestDataObject, int>> propertyExpression = x => x.IntValue;

            Assert.Throws<ArgumentNullException>(() =>
                new PropertyValidationContext<TestDataObject, int>(propertyExpression, null));
        }

        [Fact]
        public void Ctor_Call_NoException()
        {
            Expression<Func<TestDataObject, int>> propertyExpression = x => x.IntValue;

            var _ = new PropertyValidationContext<TestDataObject, int>(propertyExpression,
                A.Fake<IValidationContext<TestDataObject>>());
        }

        [Fact]
        public void PropertyValue_StrProperty_ReturnsStrValue()
        {
            var testData = new TestDataObject
            {
                StrValue = "Test text"
            };

            var validationContext = A.Fake<IValidationContext<TestDataObject>>();

            A.CallTo(() => validationContext.InstanceForValidation).Returns(testData);

            var propertyValidationContext =
                new PropertyValidationContext<TestDataObject, string>(x => x.StrValue, validationContext);

            var value = propertyValidationContext.PropertyValue;

            Assert.Equal("Test text", value);
        }

        [Fact]
        public void PropertyValue_GetStrPropertyMultipleTimes_ReturnsAlwaysTheSameResult()
        {
            var testData = new TestDataObject
            {
                StrValue = "Test text"
            };

            var validationContext = A.Fake<IValidationContext<TestDataObject>>();

            A.CallTo(() => validationContext.InstanceForValidation).Returns(testData);

            var propertyValidationContext =
                new PropertyValidationContext<TestDataObject, string>(x => x.StrValue, validationContext);

            var value = propertyValidationContext.PropertyValue;

            Assert.Equal("Test text", value);

            var value1 = propertyValidationContext.PropertyValue;

            Assert.Equal(value, value1);
        }

        [Fact]
        public void AddFault_AddNull_ThrowsException()
        {
            var validationContext = A.Fake<IValidationContext<TestDataObject>>();

            var propertyValidationContext =
                new PropertyValidationContext<TestDataObject, string>(x => x.StrValue, validationContext);

            Assert.Throws<ArgumentNullException>(() => propertyValidationContext.AddFault(null));
        }

        [Fact]
        public void AddFault_OneFault_FaultsHasExactOneElement()
        {
            var validationContext = A.Fake<IValidationContext<TestDataObject>>();

            var propertyValidationContext =
                new PropertyValidationContext<TestDataObject, string>(x => x.StrValue, validationContext);

            var fault = A.Fake<IValidationFault>();

            propertyValidationContext.AddFault(fault);

            Assert.Single((IEnumerable) propertyValidationContext.Faults);
            Assert.Single(propertyValidationContext.Faults, f => f == fault);
        }

        [Fact]
        public void AddFault_MultipleFault_FaultsMultipleElements()
        {
            var validationContext = A.Fake<IValidationContext<TestDataObject>>();

            var propertyValidationContext =
                new PropertyValidationContext<TestDataObject, string>(x => x.StrValue, validationContext);

            var fault = A.Fake<IValidationFault>();
            var fault1 = A.Fake<IValidationFault>();
            var fault2 = A.Fake<IValidationFault>();

            propertyValidationContext.AddFault(fault);
            propertyValidationContext.AddFault(fault1);
            propertyValidationContext.AddFault(fault2);

            Assert.Equal(new[]{fault, fault1, fault2}, propertyValidationContext.Faults);
        }

        [Fact]
        public void InstanceForValidation_Get_ReturnCorrectValue()
        {
            var testData = new TestDataObject
            {
                StrValue = "Test text"
            };

            var validationContext = A.Fake<IValidationContext<TestDataObject>>();

            A.CallTo(() => validationContext.InstanceForValidation).Returns(testData);

            var propertyValidationContext =
                new PropertyValidationContext<TestDataObject, string>(x => x.StrValue, validationContext);

            var instanceForValidation = propertyValidationContext.InstanceForValidation;

            Assert.Equal(testData, instanceForValidation);
        }

        [Fact]
        public void PropertyName_Get_ReturnsPropertyName()
        {
            var validationContext = A.Fake<IValidationContext<TestDataObject>>();

            var propertyValidationContext =
                new PropertyValidationContext<TestDataObject, string>(x => x.StrValue, validationContext);

            Assert.Equal("StrValue", propertyValidationContext.PropertyName);
        }
    }

}