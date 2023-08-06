using System.IO.Ports;

namespace CreativeCoders.IO.Ports;

public interface ISerialPort : IDisposable
{
    void Open();

    void Close();

    IEnumerable<byte> ReadAllBytes();

    event SerialDataReceivedEventHandler DataReceived;
}
