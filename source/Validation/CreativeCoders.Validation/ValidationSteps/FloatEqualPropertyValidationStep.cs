using System;

namespace CreativeCoders.Validation.ValidationSteps
{
    public class FloatEqualPropertyValidationStep<T, TProperty> : SimplePropertyValidationStepBase<T, TProperty>
        where T : class
    {
        private readonly double _compareValue;

        private readonly double _comparisonTolerance;

        private readonly bool _mustBeEqual;

        public FloatEqualPropertyValidationStep(double compareValue, double comparisonTolerance, bool mustBeEqual) : base("")
        {
            _compareValue = compareValue;
            _comparisonTolerance = comparisonTolerance;
            _mustBeEqual = mustBeEqual;
        }

        protected override bool IsValid(TProperty propertyValue)
        {
            return _mustBeEqual
                ? IsEqual(propertyValue)
                : !IsEqual(propertyValue);
        }

        private bool IsEqual(TProperty propertyValue)
        {
            switch (propertyValue)
            {
                case double doubleValue:
                    return Math.Abs(doubleValue - _compareValue) < _comparisonTolerance;
                case int intValue:
                    return Math.Abs(intValue - _compareValue) < _comparisonTolerance;
                case long longValue:
                    return Math.Abs(longValue - _compareValue) < _comparisonTolerance;
                case decimal decimalValue:
                    return Math.Abs(decimal.ToDouble(decimalValue) - _compareValue) < _comparisonTolerance;
            }

            if (GetValue<double>(propertyValue, out var value))
            {
                return Math.Abs(value - _compareValue) < _comparisonTolerance;
            }

            return false;
        }

        private static bool GetValue<TValue>(TProperty propertyValue, out TValue value)
        {
            try
            {
                value = (TValue)Convert.ChangeType(propertyValue, typeof(TValue));
                return true;
            }
            catch (FormatException)
            {
                value = default;
                return false;
            }
            catch (InvalidCastException)
            {
                value = default;
                return false;
            }
        }
    }
}