using System.Linq;
using CreativeCoders.Validation;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Validation
{
    public class BasicValidatorTests
    {
        [Fact]
        public void Validate_StrValueNotNull_StrValueNullReturnsInvalid()
        {
            var validator = new TestDataObjectValidator(v => v.RuleFor(x => x.StrValue).IsNotNull());

            var testData = new TestDataObject();

            var validationResult = validator.Validate(testData);

            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Faults);
            Assert.Equal("StrValue", validationResult.Faults.First().PropertyName);
        }

        [Fact]
        public void Validate_StrValueNotNull_StrValueNotNullReturnsValid()
        {
            var validator = new TestDataObjectValidator(v => v.RuleFor(x => x.StrValue).IsNotNull());

            var testData = new TestDataObject {StrValue = "Test"};

            var validationResult = validator.Validate(testData);

            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.Faults);
        }

        [Fact]
        public void ValidateNoneGeneric_StrValueNotNull_StrValueNullReturnsInvalid()
        {
            var validator = new TestDataObjectValidator(v => v.RuleFor(x => x.StrValue).IsNotNull());

            var testData = new TestDataObject() as object;

            var validationResult = validator.Validate(testData);

            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Faults);
            Assert.Equal("StrValue", validationResult.Faults.First().PropertyName);
        }

        [Fact]
        public void ValidateNoneGeneric_StrValueNotNull_StrValueNotNullReturnsValid()
        {
            var validator = new TestDataObjectValidator(v => v.RuleFor(x => x.StrValue).IsNotNull());

            var testData = new TestDataObject {StrValue = "Test"} as object;

            var validationResult = validator.Validate(testData);

            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.Faults);
        }

        [Fact]
        public void Validate_StrValueNotNullIntValueGreater100_StrValueNullIntValue0ReturnsInvalid()
        {
            var validator = new TestDataObjectValidator(v =>
            {
                v.RuleFor(x => x.StrValue).IsNotNull();
                v.RuleFor(x => x.IntValue).Must((x, i) => i > 100);
            });

            var testData = new TestDataObject();

            var validationResult = validator.Validate(testData);

            Assert.False(validationResult.IsValid);
            Assert.Equal(2, validationResult.Faults.Count());
            Assert.Equal("StrValue", validationResult.Faults.First().PropertyName);
            Assert.Equal("IntValue", validationResult.Faults.Last().PropertyName);
        }

        [Fact]
        public void ValidateWithBreakRuleFalse_StrValueNotNullIntValueGreater100_StrValueNullIntValue0ReturnsInvalid()
        {
            var validator = new TestDataObjectValidator(v =>
            {
                v.RuleFor(x => x.StrValue).IsNotNull().HasMinimumLength(10);
                v.RuleFor(x => x.IntValue).Must((x, i) => i > 100);
            });

            var testData = new TestDataObject
            {
                IntValue = 101
            };

            var validationResult = validator.Validate(testData, false);

            Assert.False(validationResult.IsValid);
            Assert.Equal(2, validationResult.Faults.Count());
            Assert.Equal("StrValue", validationResult.Faults.First().PropertyName);
            Assert.Equal("StrValue", validationResult.Faults.Last().PropertyName);
        }

        [Fact]
        public void ValidateWithBreakRuleTrue_StrValueNotNullIntValueGreater100_StrValueNullIntValue0ReturnsInvalid()
        {
            var validator = new TestDataObjectValidator(v =>
            {
                v.RuleFor(x => x.StrValue).IsNotNull().HasMinimumLength(10);
                v.RuleFor(x => x.IntValue).Must((x, i) => i > 100);
            });

            var testData = new TestDataObject
            {
                IntValue = 101
            };

            var validationResult = validator.Validate(testData, true);

            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Faults);
            Assert.Equal("StrValue", validationResult.Faults.First().PropertyName);
        }

        [Fact]
        public void ValidateStrValueExpr_StrValueNotNullIntValueGreater100_StrValueNullIntValue0ReturnsInvalid()
        {
            var validator = new TestDataObjectValidator(v =>
            {
                v.RuleFor(x => x.StrValue).IsNotNull();
                v.RuleFor(x => x.IntValue).Must((x, i) => i > 100);
            });

            var testData = new TestDataObject();

            var validationResult = validator.Validate(testData, x => x.StrValue);

            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Faults);
            Assert.Equal("StrValue", validationResult.Faults.First().PropertyName);
        }

        [Fact]
        public void ValidateStrValueExpr_StrValueNotNullIntValueGreater100_StrValueNotNullIntValue0ReturnsValid()
        {
            var validator = new TestDataObjectValidator(v =>
            {
                v.RuleFor(x => x.StrValue).IsNotNull();
                v.RuleFor(x => x.IntValue).Must((x, i) => i > 100);
            });

            var testData = new TestDataObject
            {
                StrValue = "Test"
            };

            var validationResult = validator.Validate(testData, x => x.StrValue);

            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.Faults);
        }

        [Fact]
        public void ValidateStrValue_StrValueNotNullIntValueGreater100_StrValueNullIntValue0ReturnsInvalid()
        {
            var validator = new TestDataObjectValidator(v =>
            {
                v.RuleFor(x => x.StrValue).IsNotNull();
                v.RuleFor(x => x.IntValue).Must((x, i) => i > 100);
            });

            var testData = new TestDataObject();

            var validationResult = validator.Validate(testData, "StrValue");

            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Faults);
            Assert.Equal("StrValue", validationResult.Faults.First().PropertyName);
        }

        [Fact]
        public void ValidateStrValue_StrValueNotNullIntValueGreater100_StrValueNotNullIntValue0ReturnsValid()
        {
            var validator = new TestDataObjectValidator(v =>
            {
                v.RuleFor(x => x.StrValue).IsNotNull();
                v.RuleFor(x => x.IntValue).Must((x, i) => i > 100);
            });

            var testData = new TestDataObject
            {
                StrValue = "Test"
            };

            var validationResult = validator.Validate(testData, "StrValue");

            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.Faults);
        }

        [Fact]
        public void Validate_StrValueNotNullIntValueGreater100_StrValueNullReturnsInvalid()
        {
            var validator = new TestDataObjectValidator(v =>
            {
                v.RuleFor(x => x.StrValue).IsNotNull();
                v.RuleFor(x => x.IntValue).Must((x, i) => i > 100);
            });

            var testData = new TestDataObject {IntValue = 101};

            var validationResult = validator.Validate(testData);

            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Faults);
            Assert.Equal("StrValue", validationResult.Faults.First().PropertyName);
        }

        [Fact]
        public void Validate_StrValueNotNullIntValueGreater100_StrValueNotNullIntValue0ReturnsInvalid()
        {
            var validator = new TestDataObjectValidator(v =>
            {
                v.RuleFor(x => x.StrValue).IsNotNull();
                v.RuleFor(x => x.IntValue).Must((x, i) => i > 100);
            });

            var testData = new TestDataObject {StrValue = "Test"};

            var validationResult = validator.Validate(testData);

            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Faults);
            Assert.Equal("IntValue", validationResult.Faults.First().PropertyName);
        }

        [Fact]
        public void Validate_StrValueNotNullIntValueGreater100_StrValueNotNullIntValue101ReturnsValid()
        {
            var validator = new TestDataObjectValidator(v =>
            {
                v.RuleFor(x => x.StrValue).IsNotNull();
                v.RuleFor(x => x.IntValue).Must((x, i) => i > 100);
            });

            var testData = new TestDataObject
            {
                StrValue = "Test",
                IntValue = 101
            };

            var validationResult = validator.Validate(testData);

            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.Faults);
        }

        [Fact]
        public void Validate_StrValueNotNullIfThenFalse_StrValueNullReturnsInvalid()
        {
            var validator = new TestDataObjectValidator(v => v.RuleFor(x => x.StrValue).IfThen(x => false).IsNotNull());

            var testData = new TestDataObject();

            var validationResult = validator.Validate(testData);

            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.Faults);
        }

        [Fact]
        public void Validate_StrValueNotNullIfThenTrue_StrValueNullReturnsInvalid()
        {
            var validator = new TestDataObjectValidator(v => v.RuleFor(x => x.StrValue).IfThen(x => true).IsNotNull());

            var testData = new TestDataObject();

            var validationResult = validator.Validate(testData);

            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Faults);
            Assert.Equal("StrValue", validationResult.Faults.First().PropertyName);
        }

        [Fact]
        public void Validate_StrValueNotNullIfThenTrue_StrValueNotNullReturnsValid()
        {
            var validator = new TestDataObjectValidator(v => v.RuleFor(x => x.StrValue).IfThen(x => true).IsNotNull());

            var testData = new TestDataObject {StrValue = "Test"};

            var validationResult = validator.Validate(testData);

            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.Faults);
        }

        [Fact]
        public void Validate_StrValueNotNullIfNotThenTrue_StrValueNullReturnsInvalid()
        {
            var validator =
                new TestDataObjectValidator(v => v.RuleFor(x => x.StrValue).IfNotThen(x => true).IsNotNull());

            var testData = new TestDataObject();

            var validationResult = validator.Validate(testData);

            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.Faults);
        }

        [Fact]
        public void Validate_StrValueNotNullIfNotThenFalse_StrValueNullReturnsInvalid()
        {
            var validator =
                new TestDataObjectValidator(v => v.RuleFor(x => x.StrValue).IfNotThen(x => false).IsNotNull());

            var testData = new TestDataObject();

            var validationResult = validator.Validate(testData);

            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Faults);
            Assert.Equal("StrValue", validationResult.Faults.First().PropertyName);
        }

        [Fact]
        public void Validate_StrValueNotNullIfNotThenFalse_StrValueNotNullReturnsValid()
        {
            var validator =
                new TestDataObjectValidator(v => v.RuleFor(x => x.StrValue).IfNotThen(x => false).IsNotNull());

            var testData = new TestDataObject {StrValue = "Test"};

            var validationResult = validator.Validate(testData);

            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.Faults);
        }

        [Fact]
        public void Validate_StrValueNotNullWithMessage_StrValueNullReturnsInvalid()
        {
            var validator = new TestDataObjectValidator(v =>
                v.RuleFor(x => x.StrValue).IsNotNull().WithMessage("Fault Test Message"));

            var testData = new TestDataObject();

            var validationResult = validator.Validate(testData);

            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Faults);
            Assert.Equal("StrValue", validationResult.Faults.First().PropertyName);
            Assert.Equal("Fault Test Message", validationResult.Faults.First().Message);
        }
    }
}