using System.Collections.Generic;

namespace CreativeCoders.Validation.ValidationSteps;

public class EqualPropertyValidationStep<T, TProperty> : SimplePropertyValidationStepBase<T, TProperty>
    where T : class
{
    private readonly TProperty _compareValue;

    private readonly bool _mustBeEqual;

    public EqualPropertyValidationStep(TProperty compareValue, bool mustBeEqual) : base(mustBeEqual
        ? $"Property value must equal '{compareValue}'"
        : $"Property value must not equal '{compareValue}'")
    {
        _compareValue = compareValue;
        _mustBeEqual = mustBeEqual;
    }

    protected override bool IsValid(TProperty propertyValue)
    {
        return _mustBeEqual
            ? Comparer<TProperty>.Default.Compare(propertyValue, _compareValue) == 0
            : Comparer<TProperty>.Default.Compare(propertyValue, _compareValue) != 0;
    }        
}