namespace CreativeCoders.Validation.ValidationSteps;

public abstract class SimplePropertyValidationStepBase<T, TProperty> : PropertyValidationStepBase<T, TProperty>
    where T : class
{
    protected SimplePropertyValidationStepBase(string faultMessage) : base(faultMessage)
    {
    }

    protected abstract bool IsValid(TProperty propertyValue);

    protected override bool IsValid(TProperty propertyValue, IPropertyValidationContext<T, TProperty> propertyValidationContext)
    {
        return IsValid(propertyValue);
    }
}