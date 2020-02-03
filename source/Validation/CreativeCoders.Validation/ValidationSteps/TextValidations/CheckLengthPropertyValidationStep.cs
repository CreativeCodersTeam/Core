using System;

namespace CreativeCoders.Validation.ValidationSteps.TextValidations
{
    public class CheckLengthPropertyValidationStep<T, TProperty> : PropertyValidationStepBase<T, TProperty>
        where T : class
    {
        private readonly Func<T, TProperty, int> _checkLengthFunc;

        public CheckLengthPropertyValidationStep(Func<T, TProperty, int> checkLengthFunc) : base("Property length check failed")
        {
            _checkLengthFunc = checkLengthFunc;
        }

        protected override bool IsValid(TProperty propertyValue, IPropertyValidationContext<T, TProperty> propertyValidationContext)
        {
            var propertyTextLength = propertyValue?.ToString().Length;

            if (!propertyTextLength.HasValue)
            {
                return false;
            }

            return propertyTextLength == _checkLengthFunc(propertyValidationContext.InstanceForValidation, propertyValue);
        }
    }
}