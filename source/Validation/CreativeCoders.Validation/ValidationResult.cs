using System.Collections.Generic;
using System.Linq;
using CreativeCoders.Core;

namespace CreativeCoders.Validation;

public class ValidationResult
{
    private readonly IValidationFault[] _faults;

    public ValidationResult(IEnumerable<IValidationFault> faults)
    {
        Ensure.IsNotNull(faults, nameof(faults));

        _faults = faults.ToArray();
        IsValid = _faults.Length == 0;
    }

    public IEnumerable<IValidationFault> Faults => _faults;

    public bool IsValid { get; }
}