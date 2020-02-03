using System;
using JetBrains.Annotations;

namespace CreativeCoders.Validation.ValidationSteps
{
    [PublicAPI]
    public class DelegatePropertyValidationStep<T, TProperty> : SimplePropertyValidationStepBase<T, TProperty>
        where T : class
    {
        private readonly Func<TProperty, bool> _validateFunc;

        public DelegatePropertyValidationStep(Func<TProperty, bool> validateFunc) : this(validateFunc, string.Empty)
        {
        }

        public DelegatePropertyValidationStep(Func<TProperty, bool> validateFunc, string faultMessage) : base(faultMessage)
        {
            _validateFunc = validateFunc;
        }

        protected override bool IsValid(TProperty propertyValue)
        {
            return _validateFunc(propertyValue);
        }
    }
}