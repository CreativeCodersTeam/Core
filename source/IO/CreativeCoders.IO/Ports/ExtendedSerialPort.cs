using System.IO.Ports;

namespace CreativeCoders.IO.Ports;

public class ExtendedSerialPort : SerialPort, ISerialPort
{
    public ExtendedSerialPort(string portName) : base(portName)
    {

    }

    public IEnumerable<byte> ReadAllBytes()
    {
        var buffer = new byte[BytesToRead];

        var bytesRead = Read(buffer, 0, buffer.Length);

        return buffer.Take(bytesRead);
    }
}