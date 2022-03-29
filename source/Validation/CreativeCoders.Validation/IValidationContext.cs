using System.Collections.Generic;
using JetBrains.Annotations;

namespace CreativeCoders.Validation;

[PublicAPI]
public interface IValidationContext<out T>
    where T : class
{
    void AddFaults(IEnumerable<IValidationFault> faults);

    T InstanceForValidation { get; }

    bool BreakRuleValidationAfterFirstFailedValidation { get; }

    IEnumerable<IValidationFault> Faults { get; }
}
