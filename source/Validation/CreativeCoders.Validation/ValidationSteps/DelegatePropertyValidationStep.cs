using System;
using JetBrains.Annotations;

namespace CreativeCoders.Validation.ValidationSteps
{
    [PublicAPI]
    public class DelegatePropertyValidationStep<T, TProperty> : SimplePropertyValidationStepBase<T, TProperty>
        where T : class
    {
        private readonly Func<TProperty, bool> _validate;

        public DelegatePropertyValidationStep(Func<TProperty, bool> validate) : this(validate, string.Empty)
        {
        }

        public DelegatePropertyValidationStep(Func<TProperty, bool> validate, string faultMessage) : base(faultMessage)
        {
            _validate = validate;
        }

        protected override bool IsValid(TProperty propertyValue)
        {
            return _validate(propertyValue);
        }
    }
}