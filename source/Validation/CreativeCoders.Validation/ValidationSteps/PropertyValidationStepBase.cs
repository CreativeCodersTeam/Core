namespace CreativeCoders.Validation.ValidationSteps;

public abstract class PropertyValidationStepBase<T, TProperty> : IPropertyValidationStep<T, TProperty>
    where T : class
{
    private readonly string _faultMessage;

    protected PropertyValidationStepBase(string faultMessage)
    {
        _faultMessage = faultMessage;
    }

    public bool Validate(IPropertyValidationContext<T, TProperty> propertyValidationContext)
    {
        if (IsValid(propertyValidationContext.PropertyValue, propertyValidationContext))
        {
            return true;
        }
            
        propertyValidationContext.AddFault(new ValidationFault(propertyValidationContext.PropertyName, _faultMessage));
        return false;
    }

    protected abstract bool IsValid(TProperty propertyValue, IPropertyValidationContext<T, TProperty> propertyValidationContext);
}