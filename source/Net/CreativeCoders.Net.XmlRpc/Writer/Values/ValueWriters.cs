using System;
using System.Collections.Generic;
using System.Linq;

namespace CreativeCoders.Net.XmlRpc.Writer.Values;

public class ValueWriters : IValueWriters
{
    private readonly List<ValueWriterBase> _writers;

    public ValueWriters()
    {
        _writers = new List<ValueWriterBase>
        {
            new Base64ValueWriter(),
            new BooleanValueWriter(),
            new DateTimeValueWriter(),
            new DoubleValueWriter(),
            new IntegerValueWriter(),
            new StringValueWriter(),
            new StructValueWriter(this),
            new ArrayValueWriter(this)
        };
    }

    public IValueWriter GetWriter(Type valueType)
    {
        return _writers.FirstOrDefault(w => w.HandlesType(valueType));
    }
}
