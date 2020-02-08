namespace CreativeCoders.Validation.ValidationSteps
{
    public interface IPropertyValidationStep<in T, in TProperty>
        where T : class
    {
        bool Validate(IPropertyValidationContext<T, TProperty> propertyValidationContext);
    }
}