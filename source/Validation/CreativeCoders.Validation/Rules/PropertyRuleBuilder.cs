using System;
using CreativeCoders.Validation.ValidationSteps;

namespace CreativeCoders.Validation.Rules;

public class PropertyRuleBuilder<T, TProperty> : IStartPropertyRuleBuilder<T, TProperty>
    where T : class
{
    private readonly IPropertyValidationRule<T, TProperty> _propertyValidationRule;

    public PropertyRuleBuilder(IPropertyValidationRule<T, TProperty> propertyValidationRule)
    {
        _propertyValidationRule = propertyValidationRule;
    }

    public void AddValidationStep(IPropertyValidationStep<T, TProperty> validationStep)
    {
        _propertyValidationRule.AddValidationStep(validationStep);
    }

    public void WithMessage(string message)
    {
        _propertyValidationRule.SetFaultMessage(message);
    }

    public IPropertyRuleBuilder<T, TProperty> IfThen(Func<T, bool> checkCondition)
    {
        _propertyValidationRule.SetCondition(checkCondition);
        return this;
    }

    public IPropertyRuleBuilder<T, TProperty> IfNotThen(Func<T, bool> checkCondition)
    {
        _propertyValidationRule.SetCondition(x => !checkCondition(x));
        return this;
    }
}