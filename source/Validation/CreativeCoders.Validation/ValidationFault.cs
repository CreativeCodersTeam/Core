namespace CreativeCoders.Validation;

public class ValidationFault : IValidationFault
{
    public ValidationFault(string message) : this(string.Empty, message) { }

    public ValidationFault(string propertyName, string message)
    {
        PropertyName = propertyName;
        Message = message;
    }

    public string Message { get; }

    public string PropertyName { get; }
}
