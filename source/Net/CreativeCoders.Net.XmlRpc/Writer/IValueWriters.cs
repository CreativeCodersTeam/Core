using System;

namespace CreativeCoders.Net.XmlRpc.Writer;

public interface IValueWriters
{
    IValueWriter GetWriter(Type valueType);
}