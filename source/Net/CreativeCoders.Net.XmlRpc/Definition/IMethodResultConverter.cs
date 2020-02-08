using System;

namespace CreativeCoders.Net.XmlRpc.Definition
{
    public interface IMethodResultConverter
    {
        object Convert(object result);

        Type CallResponseType { get; }
    }
}