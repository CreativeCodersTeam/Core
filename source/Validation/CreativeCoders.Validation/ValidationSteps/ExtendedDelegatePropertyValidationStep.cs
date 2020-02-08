using System;
using JetBrains.Annotations;

namespace CreativeCoders.Validation.ValidationSteps
{
    [PublicAPI]
    public class ExtendedDelegatePropertyValidationStep<T, TProperty> : PropertyValidationStepBase<T, TProperty>
        where T : class
    {
        private readonly Func<T, TProperty, bool> _validateFunc;

        public ExtendedDelegatePropertyValidationStep(Func<T, TProperty, bool> validateFunc) : this(validateFunc,
            string.Empty)
        {
        }

        public ExtendedDelegatePropertyValidationStep(Func<T, TProperty, bool> validateFunc, string faultMessage) : base(faultMessage)
        {
            _validateFunc = validateFunc;
        }

        protected override bool IsValid(TProperty propertyValue, IPropertyValidationContext<T, TProperty> propertyValidationContext)
        {
            return _validateFunc(propertyValidationContext.InstanceForValidation, propertyValue);
        }
    }
}