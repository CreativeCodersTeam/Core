using System.Diagnostics.CodeAnalysis;
using System.IO.Ports;

namespace CreativeCoders.IO.Ports;

[ExcludeFromCodeCoverage]
public class ExtendedSerialPort(string portName) : SerialPort(portName), ISerialPort
{
    public IEnumerable<byte> ReadAllBytes()
    {
        return SerialPortExtensions.ReadAllBytes(this);
    }
}
