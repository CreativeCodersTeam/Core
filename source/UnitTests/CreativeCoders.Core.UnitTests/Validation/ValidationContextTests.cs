using System;
using System.Collections;
using CreativeCoders.Validation;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Validation;

public class ValidationContextTests
{
    [Fact]
    public void Ctor_WithNullInstance_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() => new ValidationContext<object>(null, false));
    }

    [Fact]
    public void Ctor_WithInstance_StoresInstance()
    {
        var obj = new object();

        var context = new ValidationContext<object>(obj, false);

        Assert.Equal(obj, context.InstanceForValidation);
    }

    [Fact]
    public void AddFault_NewFault_StoresFault()
    {
        var obj = new object();
        var fault = A.Fake<ValidationFault>();

        var context = new ValidationContext<object>(obj, false);
        context.AddFaults(new[] {fault});

        Assert.Single((IEnumerable) context.Faults);
        Assert.Contains(fault, context.Faults);
    }

    [Fact]
    public void
        Ctor_BreakRuleValidationAfterFirstFailedValidationSetToFalse_BreakRuleValidationAfterFirstFailedValidationIsFalse()
    {
        var context = new ValidationContext<object>(new object(), false);

        Assert.False(context.BreakRuleValidationAfterFirstFailedValidation);
    }

    [Fact]
    public void
        Ctor_BreakRuleValidationAfterFirstFailedValidationSetToTrue_BreakRuleValidationAfterFirstFailedValidationIsTrue()
    {
        var context = new ValidationContext<object>(new object(), true);

        Assert.True(context.BreakRuleValidationAfterFirstFailedValidation);
    }
}
