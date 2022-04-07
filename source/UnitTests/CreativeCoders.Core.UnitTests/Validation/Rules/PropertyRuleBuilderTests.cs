using CreativeCoders.Validation.Rules;
using CreativeCoders.Validation.ValidationSteps;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Validation.Rules;

public class PropertyRuleBuilderTests
{
    [Fact]
    public void AddValidationStep()
    {
        var propValidationRule = A.Fake<IPropertyValidationRule<TestDataObject, int>>();
        var propValidationStep = A.Fake<IPropertyValidationStep<TestDataObject, int>>();

        var builder = new PropertyRuleBuilder<TestDataObject, int>(propValidationRule);

        builder.AddValidationStep(propValidationStep);

        A.CallTo(() => propValidationRule.AddValidationStep(propValidationStep))
            .MustHaveHappenedOnceExactly();
    }
}
