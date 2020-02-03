using System.Collections.Generic;

namespace CreativeCoders.Validation.ValidationSteps
{
    public class BetweenPropertyValidationStep<T, TProperty> : SimplePropertyValidationStepBase<T, TProperty>
        where T : class
    {
        private readonly TProperty _lowerEnd;

        private readonly TProperty _upperEnd;

        public BetweenPropertyValidationStep(TProperty lowerEnd, TProperty upperEnd) : base(
            $"Property value must be between {lowerEnd} and {upperEnd}")
        {
            _lowerEnd = lowerEnd;
            _upperEnd = upperEnd;
        }

        protected override bool IsValid(TProperty propertyValue)
        {
            var comparer = Comparer<TProperty>.Default;

            var aboveLowerEnd = comparer.Compare(propertyValue, _lowerEnd) >= 0;
            var underUpperEnd = comparer.Compare(propertyValue, _upperEnd) <= 0;

            return aboveLowerEnd && underUpperEnd;
        }
    }
}