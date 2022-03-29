using System.Reflection;

namespace CreativeCoders.Net.XmlRpc.Server;

public class MethodRegistration
{
    public MethodRegistration(string methodName, MethodInfo method, object target)
    {
        MethodName = methodName;
        Method = method;
        Target = target;
    }

    public string MethodName { get; }

    public MethodInfo Method { get; }

    public object Target { get; }
}