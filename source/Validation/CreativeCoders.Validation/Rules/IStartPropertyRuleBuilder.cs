using System;

namespace CreativeCoders.Validation.Rules;

public interface IStartPropertyRuleBuilder<out T, out TProperty> : IPropertyRuleBuilder<T, TProperty>
    where T : class
{
    IPropertyRuleBuilder<T, TProperty> IfThen(Func<T, bool> checkCondition);

    IPropertyRuleBuilder<T, TProperty> IfNotThen(Func<T, bool> checkCondition);
}