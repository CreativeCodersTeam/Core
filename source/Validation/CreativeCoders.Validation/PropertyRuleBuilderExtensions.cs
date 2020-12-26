using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CreativeCoders.Core;
using CreativeCoders.Validation.Rules;
using CreativeCoders.Validation.ValidationSteps;
using CreativeCoders.Validation.ValidationSteps.TextValidations;
using JetBrains.Annotations;

namespace CreativeCoders.Validation
{
    [PublicAPI]
    public static class PropertyRuleBuilderExtensions
    {
        public static IPropertyRuleBuilder<T, TProperty> IsNull<T, TProperty>(
            this IPropertyRuleBuilder<T, TProperty> ruleBuilder)
            where T : class
            where TProperty : class
        {
            return ruleBuilder.Must((_, property) => property == null);
        }

        public static IPropertyRuleBuilder<T, TProperty> IsNotNull<T, TProperty>(
            this IPropertyRuleBuilder<T, TProperty> ruleBuilder)
            where T : class
            where TProperty : class
        {
            return ruleBuilder.Must((_, property) => property != null);
        }

        public static IPropertyRuleBuilder<T, TProperty> IsEqual<T, TProperty>(
            this IPropertyRuleBuilder<T, TProperty> ruleBuilder, TProperty compareValue)
            where T : class
        {
            var validationStep = new EqualPropertyValidationStep<T, TProperty>(compareValue, true);
            ruleBuilder.AddValidationStep(validationStep);

            return ruleBuilder;
        }

        public static IPropertyRuleBuilder<T, TProperty> IsEqual<T, TProperty>(
            this IPropertyRuleBuilder<T, TProperty> ruleBuilder, double compareValue)
            where T : class
        {
            var validationStep = new FloatEqualPropertyValidationStep<T, TProperty>(compareValue, 0.0001, true);
            ruleBuilder.AddValidationStep(validationStep);

            return ruleBuilder;
        }

        public static IPropertyRuleBuilder<T, TProperty> IsEqual<T, TProperty>(
            this IPropertyRuleBuilder<T, TProperty> ruleBuilder, double compareValue, double comparisonTolerance)
            where T : class
        {
            var validationStep = new FloatEqualPropertyValidationStep<T, TProperty>(compareValue, comparisonTolerance, true);
            ruleBuilder.AddValidationStep(validationStep);

            return ruleBuilder;
        }

        public static IPropertyRuleBuilder<T, TProperty> IsNotEqual<T, TProperty>(
            this IPropertyRuleBuilder<T, TProperty> ruleBuilder, double compareValue)
            where T : class
        {
            var validationStep = new FloatEqualPropertyValidationStep<T, TProperty>(compareValue, 0.0001, false);
            ruleBuilder.AddValidationStep(validationStep);

            return ruleBuilder;
        }

        public static IPropertyRuleBuilder<T, TProperty> IsNotEqual<T, TProperty>(
            this IPropertyRuleBuilder<T, TProperty> ruleBuilder, double compareValue, double comparisonTolerance)
            where T : class
        {
            var validationStep = new FloatEqualPropertyValidationStep<T, TProperty>(compareValue, comparisonTolerance, false);
            ruleBuilder.AddValidationStep(validationStep);

            return ruleBuilder;
        }

        public static IPropertyRuleBuilder<T, string> IsEqual<T>(
            this IPropertyRuleBuilder<T, string> ruleBuilder, string compareValue, StringComparison stringComparison)
            where T : class
        {
            return ruleBuilder.Must((_, property) =>
                property == null && compareValue == null ||
                property?.ToString().Equals(compareValue, stringComparison) == true);
        }

        public static IPropertyRuleBuilder<T, string> IsNotEqual<T>(
            this IPropertyRuleBuilder<T, string> ruleBuilder, string compareValue, StringComparison stringComparison)
            where T : class
        {
            return ruleBuilder.Must((_, property) =>
                !(property == null && compareValue == null ||
                property?.ToString().Equals(compareValue, stringComparison) == true));
        }

        public static IPropertyRuleBuilder<T, TProperty> IsNotEqual<T, TProperty>(
            this IPropertyRuleBuilder<T, TProperty> ruleBuilder, TProperty compareValue)
            where T : class
        {
            var validationStep = new EqualPropertyValidationStep<T, TProperty>(compareValue, false);
            ruleBuilder.AddValidationStep(validationStep);

            return ruleBuilder;
        }

        public static IPropertyRuleBuilder<T, TProperty> IsTrue<T, TProperty>(
            this IPropertyRuleBuilder<T, TProperty> ruleBuilder, Func<TProperty, bool> isTrue)
            where T : class
        {
            Ensure.IsNotNull(isTrue, nameof(isTrue));

            var validationStep = new DelegatePropertyValidationStep<T, TProperty>(isTrue);
            ruleBuilder.AddValidationStep(validationStep);

            return ruleBuilder;
        }

        public static IPropertyRuleBuilder<T, TProperty> IsFalse<T, TProperty>(
            this IPropertyRuleBuilder<T, TProperty> ruleBuilder, Func<TProperty, bool> isFalse)
            where T : class
        {
            Ensure.IsNotNull(isFalse, nameof(isFalse));

            var validationStep = new DelegatePropertyValidationStep<T, TProperty>(property => !isFalse(property));
            ruleBuilder.AddValidationStep(validationStep);

            return ruleBuilder;
        }

        public static IPropertyRuleBuilder<T, TProperty> Must<T, TProperty>(
            this IPropertyRuleBuilder<T, TProperty> ruleBuilder, Func<T, TProperty, bool> validate)
            where T : class
        {
            Ensure.IsNotNull(validate, nameof(validate));

            var validationStep = new ExtendedDelegatePropertyValidationStep<T, TProperty>(validate);
            ruleBuilder.AddValidationStep(validationStep);

            return ruleBuilder;
        }

        public static IPropertyRuleBuilder<T, TProperty> MustNot<T, TProperty>(
            this IPropertyRuleBuilder<T, TProperty> ruleBuilder, Func<T, TProperty, bool> validate)
            where T : class
        {
            return ruleBuilder.Must((instance, property) => !validate(instance, property));
        }

        public static IPropertyRuleBuilder<T, TProperty> Between<T, TProperty>(
            this IPropertyRuleBuilder<T, TProperty> ruleBuilder, TProperty lowerEnd, TProperty upperEnd)
            where T : class
        {
            var validationStep = new BetweenPropertyValidationStep<T, TProperty>(lowerEnd, upperEnd);
            ruleBuilder.AddValidationStep(validationStep);

            return ruleBuilder;
        }

        public static IPropertyRuleBuilder<T, IEnumerable<TElement>> Contains<T, TElement>(
            this IPropertyRuleBuilder<T, IEnumerable<TElement>> ruleBuilder, TElement element)
            where T : class
        {
            return ruleBuilder.Must((_, property) => property.Contains(element));
        }

        public static IPropertyRuleBuilder<T, TProperty> Contains<T, TProperty, TElement>(
            this IPropertyRuleBuilder<T, TProperty> ruleBuilder, TElement element)
            where T : class
            where TProperty : IEnumerable<TElement>
        {
            return ruleBuilder.Must((_, property) => property.Contains(element));
        }

        public static IPropertyRuleBuilder<T, TProperty> Contains<T, TProperty>(
            this IPropertyRuleBuilder<T, TProperty> ruleBuilder, object element)
            where T : class
            where TProperty : IEnumerable
        {
            return ruleBuilder.Must((_, property) =>
                property
                    .Cast<object>()
                    .Any(x => Comparer.Default.Compare(x, element) == 0));
        }

        public static IPropertyRuleBuilder<T, string> Contains<T>(
            this IPropertyRuleBuilder<T, string> ruleBuilder, string text)
            where T : class
        {
            return ruleBuilder.Must((_, property) => property.Contains(text));
        }

        public static IPropertyRuleBuilder<T, TProperty> StartsWith<T, TProperty>(
            this IPropertyRuleBuilder<T, TProperty> ruleBuilder, string text)
            where T : class
        {
            var validationStep = new StartsWithPropertyValidationStep<T, TProperty>(text);
            ruleBuilder.AddValidationStep(validationStep);

            return ruleBuilder;
        }

        public static IPropertyRuleBuilder<T, TProperty> EndsWith<T, TProperty>(
            this IPropertyRuleBuilder<T, TProperty> ruleBuilder, string text)
            where T : class
        {
            var validationStep = new EndsWithPropertyValidationStep<T, TProperty>(text);
            ruleBuilder.AddValidationStep(validationStep);

            return ruleBuilder;
        }

        public static IPropertyRuleBuilder<T, TProperty> HasLength<T, TProperty>(
            this IPropertyRuleBuilder<T, TProperty> ruleBuilder, int length)
            where T : class
        {
            var validationStep = new HasLengthPropertyValidationStep<T, TProperty>(length);
            ruleBuilder.AddValidationStep(validationStep);

            return ruleBuilder;
        }

        public static IPropertyRuleBuilder<T, TProperty> HasLength<T, TProperty>(
            this IPropertyRuleBuilder<T, TProperty> ruleBuilder, int minLength, int maxLength)
            where T : class
        {
            var validationStep = new HasLengthBetweenPropertyValidationStep<T, TProperty>(minLength, maxLength);
            ruleBuilder.AddValidationStep(validationStep);

            return ruleBuilder;
        }

        public static IPropertyRuleBuilder<T, TProperty> HasLength<T, TProperty>(
            this IPropertyRuleBuilder<T, TProperty> ruleBuilder, Func<T, TProperty, int> checkLength)
            where T : class
        {
            var validationStep = new CheckLengthPropertyValidationStep<T, TProperty>(checkLength);
            ruleBuilder.AddValidationStep(validationStep);

            return ruleBuilder;
        }

        public static IPropertyRuleBuilder<T, TProperty> HasMinimumLength<T, TProperty>(
            this IPropertyRuleBuilder<T, TProperty> ruleBuilder, int length)
            where T : class
        {
            var validationStep = new HasMinimumLengthPropertyValidationStep<T, TProperty>(length);
            ruleBuilder.AddValidationStep(validationStep);

            return ruleBuilder;
        }

        public static IPropertyRuleBuilder<T, TProperty> HasMaximumLength<T, TProperty>(
            this IPropertyRuleBuilder<T, TProperty> ruleBuilder, int length)
            where T : class
        {
            var validationStep = new HasMaximumLengthPropertyValidationStep<T, TProperty>(length);
            ruleBuilder.AddValidationStep(validationStep);

            return ruleBuilder;
        }

        public static IPropertyRuleBuilder<T, TProperty> IsEmpty<T, TProperty>(
            this IPropertyRuleBuilder<T, TProperty> ruleBuilder)
            where T : class
            where TProperty : IEnumerable
        {
            return ruleBuilder.Must((_, property) => !property.Cast<object>().Any());
        }

        public static IPropertyRuleBuilder<T, TProperty> IsNotEmpty<T, TProperty>(
            this IPropertyRuleBuilder<T, TProperty> ruleBuilder)
            where T : class
            where TProperty : IEnumerable
        {
            return ruleBuilder.Must((_, property) => property.Cast<object>().Any());
        }

        public static IPropertyRuleBuilder<T, IEnumerable<TElement>> Single<T, TElement>(
            this IPropertyRuleBuilder<T, IEnumerable<TElement>> ruleBuilder)
            where T : class
        {
            return ruleBuilder.Must((_, property) => property.IsSingle());
        }

        public static IPropertyRuleBuilder<T, IList<TElement>> Single<T, TElement>(
            this IPropertyRuleBuilder<T, IList<TElement>> ruleBuilder)
            where T : class
        {
            return ruleBuilder.Must((_, property) => property.IsSingle());
        }

        public static IPropertyRuleBuilder<T, TProperty> Count<T, TProperty>(
            this IPropertyRuleBuilder<T, TProperty> ruleBuilder, int count)
            where T : class
            where TProperty : IEnumerable
        {
            return ruleBuilder.Must((_, property) => property.Cast<object>().Count() == count);
        }

        public static IPropertyRuleBuilder<T, IEnumerable<TElement>> Count<T, TElement>(
            this IPropertyRuleBuilder<T, IEnumerable<TElement>> ruleBuilder, int count)
            where T : class
        {
            return ruleBuilder.Must((_, property) => property.Count() == count);
        }

        public static IPropertyRuleBuilder<T, IEnumerable<TElement>> All<T, TElement>(
            this IPropertyRuleBuilder<T, IEnumerable<TElement>> ruleBuilder, Func<TElement, bool> predicate)
            where T : class
        {
            return ruleBuilder.Must((_, property) => property.All(predicate));
        }

        public static IPropertyRuleBuilder<T, IList<TElement>> All<T, TElement>(
            this IPropertyRuleBuilder<T, IList<TElement>> ruleBuilder, Func<TElement, bool> predicate)
            where T : class
        {
            return ruleBuilder.Must((_, property) => property.All(predicate));
        }

        public static IPropertyRuleBuilder<T, IEnumerable<TElement>> Any<T, TElement>(
            this IPropertyRuleBuilder<T, IEnumerable<TElement>> ruleBuilder, Func<TElement, bool> predicate)
            where T : class
        {
            return ruleBuilder.Must((_, property) => property.Any(predicate));
        }

        public static IPropertyRuleBuilder<T, IList<TElement>> Any<T, TElement>(
            this IPropertyRuleBuilder<T, IList<TElement>> ruleBuilder, Func<TElement, bool> predicate)
            where T : class
        {
            return ruleBuilder.Must((_, property) => property.Any(predicate));
        }

        public static IPropertyRuleBuilder<T, TProperty> IsNullOrWhiteSpace<T, TProperty>(
            this IPropertyRuleBuilder<T, TProperty> ruleBuilder)
            where T : class
        {
            return ruleBuilder.IsTrue(property => string.IsNullOrWhiteSpace(property?.ToString()));
        }

        public static IPropertyRuleBuilder<T, TProperty> IsNotNullOrWhiteSpace<T, TProperty>(
            this IPropertyRuleBuilder<T, TProperty> ruleBuilder)
            where T : class
        {
            return ruleBuilder.IsFalse(property => string.IsNullOrWhiteSpace(property?.ToString()));
        }
    }
}
