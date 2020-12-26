using System;
using JetBrains.Annotations;

namespace CreativeCoders.Net.XmlRpc.Definition
{
    [PublicAPI]
    public interface IMethodResultConverter
    {
        object Convert(object result);

        //todo implement
        Type CallResponseType { get; }
    }
}
