using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CreativeCoders.Core;
using CreativeCoders.Core.Reflection;

namespace CreativeCoders.Validation;

public class PropertyValidationContext<T, TProperty> : IPropertyValidationContext<T, TProperty>
    where T : class
{
    private readonly Expression<Func<T, TProperty>> _propertyExpression;

    private readonly IValidationContext<T> _validationContext;

    private readonly IList<IValidationFault> _faults;

    private readonly Lazy<TProperty> _propertyValue;

    private readonly Lazy<string> _propertyName;

    public PropertyValidationContext(Expression<Func<T, TProperty>> propertyExpression,
        IValidationContext<T> validationContext)
    {
        Ensure.IsNotNull(propertyExpression, nameof(propertyExpression));
        Ensure.That(propertyExpression.IsPropertyOf(), nameof(propertyExpression),
            "{nameof(propertyExpression)} must be a property in class {typeof(T).Name}");
        Ensure.IsNotNull(validationContext, nameof(validationContext));

        _propertyExpression = propertyExpression;
        _validationContext = validationContext;
        _faults = new List<IValidationFault>();
        _propertyValue = new Lazy<TProperty>(GetValue);
        _propertyName = new Lazy<string>(() => _propertyExpression.GetMemberName());
    }

    private TProperty GetValue()
    {
        var getPropertyValue = _propertyExpression.Compile();
        return getPropertyValue(_validationContext.InstanceForValidation);
    }

    public void AddFault(IValidationFault fault)
    {
        Ensure.IsNotNull(fault, nameof(fault));

        _faults.Add(fault);
    }

    public IEnumerable<IValidationFault> Faults => _faults;

    public T InstanceForValidation => _validationContext.InstanceForValidation;

    public TProperty PropertyValue => _propertyValue.Value;

    public string PropertyName => _propertyName.Value;
}
