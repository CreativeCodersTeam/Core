using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CreativeCoders.Core;
using CreativeCoders.Core.Reflection;
using CreativeCoders.Validation.Rules;

namespace CreativeCoders.Validation;

public abstract class ValidatorBase<T> : IValidator<T>, IValidator
    where T : class
{
    private readonly IList<IValidationRule<T>> _rules;

    protected ValidatorBase()
    {
        _rules = new List<IValidationRule<T>>();
    }

    public ValidationResult Validate(T instanceForValidation)
    {
        return Validate(instanceForValidation, true);
    }

    public ValidationResult Validate(T instanceForValidation,
        bool breakRuleValidationAfterFirstFailedValidation)
    {
        Ensure.IsNotNull(instanceForValidation, nameof(instanceForValidation));

        return ValidateRules(instanceForValidation, _rules, breakRuleValidationAfterFirstFailedValidation);
    }

    public ValidationResult Validate<TProperty>(T instanceForValidation,
        Expression<Func<T, TProperty>> propertyExpression)
    {
        return Validate(instanceForValidation, propertyExpression, true);
    }

    public ValidationResult Validate<TProperty>(T instanceForValidation,
        Expression<Func<T, TProperty>> propertyExpression,
        bool breakRuleValidationAfterFirstFailedValidation)
    {
        Ensure.IsNotNull(instanceForValidation, nameof(instanceForValidation));
        Ensure.IsNotNull(propertyExpression, nameof(propertyExpression));
        Ensure.That(propertyExpression.IsPropertyOf(), nameof(propertyExpression),
            $"{nameof(propertyExpression)} must be a property in class {typeof(T).Name}");

        return ValidateRules(instanceForValidation, _rules.Where(rule => rule.Affects(propertyExpression)),
            breakRuleValidationAfterFirstFailedValidation);
    }

    public IStartPropertyRuleBuilder<T, TProperty> RuleFor<TProperty>(
        Expression<Func<T, TProperty>> propertyExpression)
    {
        Ensure.IsNotNull(propertyExpression, nameof(propertyExpression));
        Ensure.That(propertyExpression.IsPropertyOf(), nameof(propertyExpression),
            $"{nameof(propertyExpression)} must be a property in class {typeof(T).Name}");

        var rule = new PropertyValidationRule<T, TProperty>(propertyExpression);
        _rules.Add(rule);

        var ruleBuilder = new PropertyRuleBuilder<T, TProperty>(rule);

        return ruleBuilder;
    }

    public ValidationResult Validate(object instanceForValidation)
    {
        return Validate(instanceForValidation, true);
    }

    public ValidationResult Validate(object instanceForValidation,
        bool breakRuleValidationAfterFirstFailedValidation)
    {
        Ensure.IsNotNull(instanceForValidation, nameof(instanceForValidation));
        Ensure.That(CanValidate(instanceForValidation), nameof(instanceForValidation),
            $"Validator not responsible for '{instanceForValidation.GetType().Name}'");

        return Validate((T) instanceForValidation, breakRuleValidationAfterFirstFailedValidation);
    }

    public ValidationResult Validate(object instanceForValidation, string propertyName)
    {
        return Validate(instanceForValidation, propertyName, true);
    }

    public ValidationResult Validate(object instanceForValidation, string propertyName,
        bool breakRuleValidationAfterFirstFailedValidation)
    {
        Ensure.IsNotNull(instanceForValidation, nameof(instanceForValidation));
        Ensure.That(CanValidate(instanceForValidation), nameof(instanceForValidation),
            $"Validator not responsible for '{instanceForValidation.GetType().Name}'");
        Ensure.IsNotNullOrWhitespace(propertyName, nameof(propertyName));

        return ValidateRules((T) instanceForValidation, _rules.Where(rule => rule.Affects(propertyName)),
            breakRuleValidationAfterFirstFailedValidation);
    }

    private static ValidationResult ValidateRules(T instanceForValidation,
        IEnumerable<IValidationRule<T>> rules,
        bool breakRuleValidationAfterFirstFailedValidation)
    {
        var validationContext =
            new ValidationContext<T>(instanceForValidation, breakRuleValidationAfterFirstFailedValidation);

        foreach (var validationRule in rules)
        {
            validationRule.Validate(validationContext);
        }

        return new ValidationResult(validationContext.Faults);
    }

    private static bool CanValidate(object instance)
    {
        return typeof(T).GetTypeInfo().IsAssignableFrom(instance?.GetType().GetTypeInfo());
    }
}
