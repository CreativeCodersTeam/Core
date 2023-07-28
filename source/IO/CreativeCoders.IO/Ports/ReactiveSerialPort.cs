using System.IO.Ports;
using System.Reactive.Linq;
using CreativeCoders.Core;
using JetBrains.Annotations;

namespace CreativeCoders.IO.Ports;

[PublicAPI]
public sealed class ReactiveSerialPort : IObservable<byte[]>, IDisposable
{
    private readonly ISerialPort _serialPort;

    private readonly IObservable<byte[]> _dataObservable;

    public ReactiveSerialPort(string portName)
        : this(new ExtendedSerialPort(Ensure.NotNull(portName, nameof(portName))))
    {

    }

    public ReactiveSerialPort(ISerialPort serialPort)
    {
        _serialPort = Ensure.NotNull(serialPort, nameof(serialPort));

        _dataObservable = _serialPort.ToDataObservable();
    }

    public void Open()
    {
        _serialPort.Open();
    }

    public void Close()
    {
        _serialPort.Close();
    }

    public IDisposable Subscribe(IObserver<byte[]> observer)
    {
        return _dataObservable.Subscribe(observer);
    }

    public void Dispose()
    {
        _serialPort.Dispose();
    }
}
