using System.Collections.Generic;
using JetBrains.Annotations;

namespace CreativeCoders.Net.XmlRpc.Server;

[PublicAPI]
public interface IXmlRpcServerMethods
{
    void RegisterMethods<T>(string methodSuffix, T methodsInterface)
        where T : class;

    void RegisterMethods<T>(T methodsInterface)
        where T : class;

    MethodRegistration GetMethod(string methodName);

    IEnumerable<MethodRegistration> Methods { get; }
}
