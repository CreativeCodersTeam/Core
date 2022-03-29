using JetBrains.Annotations;

namespace CreativeCoders.Validation;

[PublicAPI]
public interface IValidator
{
    ValidationResult Validate(object instanceForValidation);

    ValidationResult Validate(object instanceForValidation,
        bool breakRuleValidationAfterFirstFailedValidation);

    ValidationResult Validate(object instanceForValidation, string propertyName);

    ValidationResult Validate(object instanceForValidation, string propertyName,
        bool breakRuleValidationAfterFirstFailedValidation);
}
