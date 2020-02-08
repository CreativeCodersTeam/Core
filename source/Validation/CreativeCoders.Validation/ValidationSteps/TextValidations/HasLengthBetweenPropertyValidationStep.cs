namespace CreativeCoders.Validation.ValidationSteps.TextValidations
{
    public class HasLengthBetweenPropertyValidationStep<T, TProperty> : SimplePropertyValidationStepBase<T, TProperty>
        where T : class
    {
        private readonly int _minLength;

        private readonly int _maxLength;

        public HasLengthBetweenPropertyValidationStep(int minLength, int maxLength) : base(
            $"Property must have a length between {minLength} and {maxLength}")
        {
            _minLength = minLength;
            _maxLength = maxLength;
        }

        protected override bool IsValid(TProperty propertyValue)
        {
            var text = propertyValue?.ToString();
            var textLength = text?.Length ?? 0;

            return textLength >= _minLength && textLength <= _maxLength;
        }
    }
}