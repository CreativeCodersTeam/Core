using System;

namespace CreativeCoders.Validation.ValidationSteps.TextValidations;

public class CheckLengthPropertyValidationStep<T, TProperty> : PropertyValidationStepBase<T, TProperty>
    where T : class
{
    private readonly Func<T, TProperty, int> _checkLength;

    public CheckLengthPropertyValidationStep(Func<T, TProperty, int> checkLength) : base("Property length check failed")
    {
        _checkLength = checkLength;
    }

    protected override bool IsValid(TProperty propertyValue, IPropertyValidationContext<T, TProperty> propertyValidationContext)
    {
        var propertyTextLength = propertyValue?.ToString()?.Length;

        if (!propertyTextLength.HasValue)
        {
            return false;
        }

        return propertyTextLength == _checkLength(propertyValidationContext.InstanceForValidation, propertyValue);
    }
}