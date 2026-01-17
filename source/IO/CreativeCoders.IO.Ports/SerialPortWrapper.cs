using System.Diagnostics.CodeAnalysis;
using System.IO.Ports;
using CreativeCoders.Core;
using JetBrains.Annotations;

namespace CreativeCoders.IO.Ports;

[ExcludeFromCodeCoverage]
[PublicAPI]
public sealed class SerialPortWrapper(SerialPort serialPort) : ISerialPort
{
    private readonly SerialPort _serialPort = Ensure.NotNull(serialPort);

    public void Dispose()
    {
        _serialPort.Dispose();
    }

    public void Open()
    {
        _serialPort.Open();
    }

    public void Close()
    {
        _serialPort.Close();
    }

    public IEnumerable<byte> ReadAllBytes()
    {
        return _serialPort.ReadAllBytes();
    }

    public event SerialDataReceivedEventHandler? DataReceived
    {
        add => _serialPort.DataReceived += value;
        remove => _serialPort.DataReceived -= value;
    }
}
