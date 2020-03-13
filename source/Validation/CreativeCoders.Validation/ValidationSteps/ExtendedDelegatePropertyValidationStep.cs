using System;
using JetBrains.Annotations;

namespace CreativeCoders.Validation.ValidationSteps
{
    [PublicAPI]
    public class ExtendedDelegatePropertyValidationStep<T, TProperty> : PropertyValidationStepBase<T, TProperty>
        where T : class
    {
        private readonly Func<T, TProperty, bool> _validate;

        public ExtendedDelegatePropertyValidationStep(Func<T, TProperty, bool> validate) : this(validate,
            string.Empty)
        {
        }

        public ExtendedDelegatePropertyValidationStep(Func<T, TProperty, bool> validate, string faultMessage) : base(faultMessage)
        {
            _validate = validate;
        }

        protected override bool IsValid(TProperty propertyValue, IPropertyValidationContext<T, TProperty> propertyValidationContext)
        {
            return _validate(propertyValidationContext.InstanceForValidation, propertyValue);
        }
    }
}