using System;
using System.Collections;
using CreativeCoders.Validation;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Validation
{
    public class ValidationResultTests
    {
        [Fact]
        public void Ctor_Call_ThrowsNoException()
        {
            var _ = new ValidationResult(Array.Empty<ValidationFault>());
        }

        [Fact]
        public void Ctor_CallWithFaultsParam_FaultsStored()
        {
            var validationResult = new ValidationResult(new []{new ValidationFault("Test")});

            Assert.Single((IEnumerable) validationResult.Faults);
        }

        [Fact]
        public void Ctor_CallWithFaultsParam_IsValidFalse()
        {
            var validationResult = new ValidationResult(new[] { new ValidationFault("Test") });

            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public void Ctor_CallWithEmptyFaultsParam_IsValidTrue()
        {
            var validationResult = new ValidationResult(Array.Empty<ValidationFault>());

            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public void Ctor_CallWithNullFaultsParam_IsValidTrue()
        {
            var validationResult = new ValidationResult(Array.Empty<ValidationFault>());

            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.Faults);
        }
    }
}
