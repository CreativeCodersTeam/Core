using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CreativeCoders.Core;
using CreativeCoders.Core.Reflection;
using CreativeCoders.Validation.ValidationSteps;

namespace CreativeCoders.Validation.Rules;

public class PropertyValidationRule<T, TProperty> : IValidationRule<T>, IPropertyValidationRule<T, TProperty>
    where T : class
{
    private readonly IList<IPropertyValidationStep<T, TProperty>> _validationSteps;

    private readonly Expression<Func<T, TProperty>> _propertyExpression;

    private Func<T, bool> _checkCondition;

    public PropertyValidationRule(Expression<Func<T, TProperty>> propertyExpression)
    {
        Ensure.IsNotNull(propertyExpression, nameof(propertyExpression));
        Ensure.That(propertyExpression.IsPropertyOf(), nameof(propertyExpression),
            $"{nameof(propertyExpression)} must be a property in class {typeof(T).Name}");

        _propertyExpression = propertyExpression;
        _validationSteps = new List<IPropertyValidationStep<T, TProperty>>();
    }

    public void Validate(IValidationContext<T> validationContext)
    {
        if (_checkCondition?.Invoke(validationContext.InstanceForValidation) == false)
        {
            return;
        }

        var propertyValidationContext = new PropertyValidationContext<T, TProperty>(_propertyExpression, validationContext);

        // ReSharper disable once LoopCanBePartlyConvertedToQuery
        foreach (var propertyValidationStep in _validationSteps)
        {
            if (!propertyValidationStep.Validate(propertyValidationContext) &&
                validationContext.BreakRuleValidationAfterFirstFailedValidation)
            {
                break;
            }
        }

        if (!propertyValidationContext.Faults.Any()) return;

        validationContext.AddFaults(HasFaultMessage
            ? new[] {new ValidationFault(propertyValidationContext.PropertyName, FaultMessage)}
            : propertyValidationContext.Faults);
    }

    public bool Affects<TProp>(Expression<Func<T, TProp>> propertyExpression)
    {
        if (!propertyExpression.IsPropertyOf())
        {
            return false;
        }

        return propertyExpression.GetMemberName() == _propertyExpression.GetMemberName();
    }

    public bool Affects(string propertyName)
    {
        return _propertyExpression.GetMemberName() == propertyName;
    }

    public void AddValidationStep(IPropertyValidationStep<T, TProperty> validationStep)
    {
        _validationSteps.Add(validationStep);
    }

    public void SetFaultMessage(string message)
    {
        HasFaultMessage = true;
        FaultMessage = message;
    }

    public string FaultMessage { get; private set; }

    public bool HasFaultMessage { get; private set; }

    public void SetCondition(Func<T, bool> checkCondition)
    {
        _checkCondition = checkCondition;
    }
}