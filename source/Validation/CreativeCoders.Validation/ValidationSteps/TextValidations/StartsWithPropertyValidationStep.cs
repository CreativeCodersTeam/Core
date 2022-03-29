using CreativeCoders.Core;

namespace CreativeCoders.Validation.ValidationSteps.TextValidations;

public class StartsWithPropertyValidationStep<T, TProperty> : SimplePropertyValidationStepBase<T, TProperty>
    where T : class
{
    private readonly string _text;

    public StartsWithPropertyValidationStep(string text) : base($"Property must start with '{text}'")
    {
        Ensure.IsNotNullOrEmpty(text, nameof(text));

        _text = text;
    }

    protected override bool IsValid(TProperty propertyValue)
    {
        var propertyText = propertyValue?.ToString();

        return propertyText?.StartsWith(_text) == true;
    }
}