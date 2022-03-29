using System;
using System.Linq.Expressions;

namespace CreativeCoders.Validation.Rules;

public interface IValidationRule<T>
    where T : class
{
    void Validate(IValidationContext<T> validationContext);

    bool Affects<TProperty>(Expression<Func<T, TProperty>> propertyExpression);

    bool Affects(string propertyName);
}