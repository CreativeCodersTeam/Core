using System.Collections.Generic;
using JetBrains.Annotations;

namespace CreativeCoders.Validation;

[PublicAPI]
public interface IPropertyValidationContext<out T, out TProperty>
    where T : class
{
    void AddFault(IValidationFault fault);

    IEnumerable<IValidationFault> Faults { get; }

    T InstanceForValidation { get; }

    TProperty PropertyValue { get; }

    string PropertyName { get; }
}
