namespace CreativeCoders.Validation.ValidationSteps.TextValidations;

public class
    HasMinimumLengthPropertyValidationStep<T, TProperty> : SimplePropertyValidationStepBase<T, TProperty>
    where T : class
{
    private readonly int _length;

    public HasMinimumLengthPropertyValidationStep(int length) : base(
        $"Property must have at least the length {length}")
    {
        _length = length;
    }

    protected override bool IsValid(TProperty propertyValue)
    {
        var text = propertyValue?.ToString();
        var textLength = text?.Length ?? 0;

        return textLength >= _length;
    }
}
