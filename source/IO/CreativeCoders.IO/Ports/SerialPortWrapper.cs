using System.IO.Ports;
using CreativeCoders.Core;

namespace CreativeCoders.IO.Ports;

public class SerialPortWrapper : ISerialPort
{
    private readonly SerialPort _serialPort;

    public SerialPortWrapper(SerialPort serialPort)
    {
        _serialPort = Ensure.NotNull(serialPort);
    }

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
        throw new NotImplementedException();
    }

    public event SerialDataReceivedEventHandler? DataReceived
    {
        add => _serialPort.DataReceived += value;
        remove => _serialPort.DataReceived -= value;
    }
}