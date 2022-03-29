using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CreativeCoders.Validation;
using CreativeCoders.Validation.Rules;
using CreativeCoders.Validation.ValidationSteps;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Validation.Rules;

[SuppressMessage("ReSharper", "ImplicitlyCapturedClosure")]
public class PropertyValidationRuleTests
{
    [Fact]
    public void Ctor_CallWithExpressionNull_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() => new PropertyValidationRule<TestDataObject, string>(null));
    }

    [Fact]
    public void Ctor_CallWithPropertyFromWrongClass_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() =>
            new PropertyValidationRule<TestDataObject, int>(x => "Test".Length));
    }

    [Fact]
    public void Ctor_Call_NoException()
    {
        var _ = new PropertyValidationRule<TestDataObject, string>(x => x.StrValue);
    }

    [Fact]
    public void Affects_CallWithPropertyExpression_ReturnsTrue()
    {
        var rule = new PropertyValidationRule<TestDataObject, string>(x => x.StrValue);

        Assert.True(rule.Affects(x => x.StrValue));
    }

    [Fact]
    public void Affects_CallWithWrongPropertyExpression_ReturnsFalse()
    {
        var rule = new PropertyValidationRule<TestDataObject, string>(x => x.StrValue);

        Assert.False(rule.Affects(x => x.IntValue));
    }

    [Fact]
    public void Affects_CallWithPropertyExpressionFromWrongClass_ReturnsFalse()
    {
        var rule = new PropertyValidationRule<TestDataObject, string>(x => x.StrValue);

        Assert.False(rule.Affects(x => "Test".Length));
    }

    [Fact]
    public void Affects_CallWithPropertyName_ReturnsTrue()
    {
        var rule = new PropertyValidationRule<TestDataObject, string>(x => x.StrValue);

        Assert.True(rule.Affects("StrValue"));
    }

    [Fact]
    public void Affects_CallWithWrongPropertyName_ReturnsFalse()
    {
        var rule = new PropertyValidationRule<TestDataObject, string>(x => x.StrValue);

        Assert.False(rule.Affects("IntValue"));
    }

    [Fact]
    public void SetFaultMessage_Text_HasFaultMessageIsTrueAndFaultMessageIsSet()
    {
        var rule = new PropertyValidationRule<TestDataObject, string>(x => x.StrValue);

        Assert.False(rule.HasFaultMessage);
        Assert.Null(rule.FaultMessage);

        rule.SetFaultMessage("Fault text");

        Assert.True(rule.HasFaultMessage);
        Assert.Equal("Fault text", rule.FaultMessage);
    }

    [Fact]
    public void Validate_NoSteps_DoesNothing()
    {
        var validationContext = A.Fake<IValidationContext<TestDataObject>>();
            
        var rule = new PropertyValidationRule<TestDataObject, string>(x => x.StrValue);

        rule.Validate(validationContext);

        A.CallTo(() => validationContext.AddFaults(A<IEnumerable<IValidationFault>>._)).MustNotHaveHappened();
    }

    [Fact]
    public void Validate_OneStep_DoesStepValidation()
    {
        var validationContext = A.Fake<IValidationContext<TestDataObject>>();
        var validationStep = A.Fake<IPropertyValidationStep<TestDataObject, string>>();
        var testData = A.Fake<TestDataObject>();
        testData.StrValue = "Test";

        A.CallTo(() => validationContext.InstanceForValidation).Returns(testData);

        var rule = new PropertyValidationRule<TestDataObject, string>(x => x.StrValue);
        rule.AddValidationStep(validationStep);


        rule.Validate(validationContext);


        A.CallTo(() =>
                validationStep.Validate(
                    A<IPropertyValidationContext<TestDataObject, string>>.That.Matches(x =>
                        x.InstanceForValidation == testData)))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void Validate_TwoSteps_DoesStepsValidation()
    {
        var validationContext = A.Fake<IValidationContext<TestDataObject>>();
        var validationStep = A.Fake<IPropertyValidationStep<TestDataObject, string>>();
        var validationStep2 = A.Fake<IPropertyValidationStep<TestDataObject, string>>();
        var testData = A.Fake<TestDataObject>();
        testData.StrValue = "Test";

        A.CallTo(() => validationContext.InstanceForValidation).Returns(testData);

        var rule = new PropertyValidationRule<TestDataObject, string>(x => x.StrValue);
        rule.AddValidationStep(validationStep);
        rule.AddValidationStep(validationStep2);


        rule.Validate(validationContext);


        A.CallTo(() =>
                validationStep.Validate(
                    A<IPropertyValidationContext<TestDataObject, string>>.That.Matches(x =>
                        x.InstanceForValidation == testData)))
            .MustHaveHappenedOnceExactly();

        A.CallTo(() =>
                validationStep2.Validate(
                    A<IPropertyValidationContext<TestDataObject, string>>.That.Matches(x =>
                        x.InstanceForValidation == testData)))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void Validate_TwoStepsWithBreak_DoesStepsValidationOnlyForFirstStep()
    {
        var validationContext = A.Fake<IValidationContext<TestDataObject>>();
        var validationStep = A.Fake<IPropertyValidationStep<TestDataObject, string>>();
        var validationStep2 = A.Fake<IPropertyValidationStep<TestDataObject, string>>();
        var testData = A.Fake<TestDataObject>();
        testData.StrValue = "Test";

        A.CallTo(() => validationContext.InstanceForValidation).Returns(testData);
        A.CallTo(() => validationContext.BreakRuleValidationAfterFirstFailedValidation).Returns(true);

        var rule = new PropertyValidationRule<TestDataObject, string>(x => x.StrValue);
        rule.AddValidationStep(validationStep);
        rule.AddValidationStep(validationStep2);


        rule.Validate(validationContext);


        A.CallTo(() =>
                validationStep.Validate(
                    A<IPropertyValidationContext<TestDataObject, string>>.That.Matches(x =>
                        x.InstanceForValidation == testData)))
            .MustHaveHappenedOnceExactly();

        A.CallTo(() =>
                validationStep2.Validate(
                    A<IPropertyValidationContext<TestDataObject, string>>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public void Validate_OneStepWithFalseCondition_DoesNoStepValidation()
    {
        var validationContext = A.Fake<IValidationContext<TestDataObject>>();
        var validationStep = A.Fake<IPropertyValidationStep<TestDataObject, string>>();
        var testData = A.Fake<TestDataObject>();
        testData.StrValue = "Test";

        A.CallTo(() => validationContext.InstanceForValidation).Returns(testData);

        var rule = new PropertyValidationRule<TestDataObject, string>(x => x.StrValue);
        rule.AddValidationStep(validationStep);
        rule.SetCondition(_ => false);


        rule.Validate(validationContext);


        A.CallTo(() =>
                validationStep.Validate(
                    A<IPropertyValidationContext<TestDataObject, string>>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public void Validate_OneStepWithTrueCondition_DoesStepValidation()
    {
        var validationContext = A.Fake<IValidationContext<TestDataObject>>();
        var validationStep = A.Fake<IPropertyValidationStep<TestDataObject, string>>();
        var testData = A.Fake<TestDataObject>();
        testData.StrValue = "Test";

        A.CallTo(() => validationContext.InstanceForValidation).Returns(testData);

        var rule = new PropertyValidationRule<TestDataObject, string>(x => x.StrValue);
        rule.AddValidationStep(validationStep);
        rule.SetCondition(_ => true);


        rule.Validate(validationContext);


        A.CallTo(() =>
                validationStep.Validate(
                    A<IPropertyValidationContext<TestDataObject, string>>._))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void Validate_OneStep_AddFaults()
    {
        var validationContext = A.Fake<IValidationContext<TestDataObject>>();
        var validationStep = A.Fake<IPropertyValidationStep<TestDataObject, string>>();
        var testData = A.Fake<TestDataObject>();
        testData.StrValue = "Test";
        var validationFault = A.Fake<IValidationFault>();

        A.CallTo(() => validationContext.InstanceForValidation).Returns(testData);
        A.CallTo(() => validationStep.Validate(A<IPropertyValidationContext<TestDataObject, string>>._))
            .Invokes((IPropertyValidationContext<TestDataObject, string> context) =>
                context.AddFault(validationFault))
            .Returns(false);

        var rule = new PropertyValidationRule<TestDataObject, string>(x => x.StrValue);
        rule.AddValidationStep(validationStep);


        rule.Validate(validationContext);


        A.CallTo(() =>
                validationContext.AddFaults(A<IEnumerable<IValidationFault>>.That.IsSameSequenceAs(validationFault)))
            .MustHaveHappenedOnceExactly();
    }
}