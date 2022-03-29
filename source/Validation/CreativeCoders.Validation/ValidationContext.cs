using System.Collections.Generic;
using CreativeCoders.Core;

namespace CreativeCoders.Validation;

public class ValidationContext<T> : IValidationContext<T>
    where T : class
{
    private readonly IList<IValidationFault> _faults;

    public ValidationContext(T instanceForValidation, bool breakRuleValidationAfterFirstFailedValidation)
    {
        Ensure.IsNotNull(instanceForValidation, nameof(instanceForValidation));

        InstanceForValidation = instanceForValidation;
        BreakRuleValidationAfterFirstFailedValidation = breakRuleValidationAfterFirstFailedValidation;
        _faults = new List<IValidationFault>();
    }

    public void AddFaults(IEnumerable<IValidationFault> faults)
    {
        foreach (var validationFault in faults)
        {
            _faults.Add(validationFault);
        }
    }

    public T InstanceForValidation { get; }

    public bool BreakRuleValidationAfterFirstFailedValidation { get; }

    public IEnumerable<IValidationFault> Faults => _faults;
}