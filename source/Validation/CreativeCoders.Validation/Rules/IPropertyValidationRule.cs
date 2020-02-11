using System;
using CreativeCoders.Validation.ValidationSteps;

namespace CreativeCoders.Validation.Rules
{
    public interface IPropertyValidationRule<out T, out TProperty>
        where T : class
    {
        void AddValidationStep(IPropertyValidationStep<T, TProperty> validationStep);

        void SetFaultMessage(string message);

        void SetCondition(Func<T, bool> checkCondition);
    }
}