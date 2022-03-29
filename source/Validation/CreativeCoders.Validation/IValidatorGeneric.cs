using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace CreativeCoders.Validation;

[PublicAPI]
public interface IValidator<T>
    where T : class
{
    ValidationResult Validate(T instanceForValidation);

    ValidationResult Validate(T instanceForValidation, bool breakRuleValidationAfterFirstFailedValidation);

    ValidationResult Validate<TProperty>(T instanceForValidation,
        Expression<Func<T, TProperty>> propertyExpression);

    ValidationResult Validate<TProperty>(T instanceForValidation,
        Expression<Func<T, TProperty>> propertyExpression,
        bool breakRuleValidationAfterFirstFailedValidation);
}
