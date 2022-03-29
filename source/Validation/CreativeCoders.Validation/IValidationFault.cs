namespace CreativeCoders.Validation;

public interface IValidationFault
{
    string Message { get; }

    string PropertyName { get; }
}