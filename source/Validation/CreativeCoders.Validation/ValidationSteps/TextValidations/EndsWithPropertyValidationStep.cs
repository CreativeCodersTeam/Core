using CreativeCoders.Core;

namespace CreativeCoders.Validation.ValidationSteps.TextValidations;

public class EndsWithPropertyValidationStep<T, TProperty> : SimplePropertyValidationStepBase<T, TProperty>
    where T : class
{
    private readonly string _text;

    public EndsWithPropertyValidationStep(string text) : base($"Property must end with '{text}'")
    {
        Ensure.IsNotNullOrEmpty(text, nameof(text));

        _text = text;
    }

    protected override bool IsValid(TProperty propertyValue)
    {
        var propertyText = propertyValue?.ToString();

        return propertyText?.EndsWith(_text) == true;
    }
}
