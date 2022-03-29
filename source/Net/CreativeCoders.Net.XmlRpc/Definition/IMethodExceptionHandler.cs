namespace CreativeCoders.Net.XmlRpc.Definition;

public interface IMethodExceptionHandler
{
    void HandleException(MethodExceptionHandlerArguments arguments);
}