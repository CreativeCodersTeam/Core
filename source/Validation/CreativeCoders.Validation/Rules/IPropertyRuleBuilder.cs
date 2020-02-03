using CreativeCoders.Validation.ValidationSteps;

namespace CreativeCoders.Validation.Rules
{
    public interface IPropertyRuleBuilder<out T, out TProperty>
        where T : class
    {
        void AddValidationStep(IPropertyValidationStep<T, TProperty> validationStep);

        void WithMessage(string message);
    }
}