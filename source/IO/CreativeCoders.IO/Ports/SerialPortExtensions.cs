using System.Diagnostics.CodeAnalysis;
using System.IO.Ports;
using System.Reactive.Linq;
using JetBrains.Annotations;

namespace CreativeCoders.IO.Ports;

[PublicAPI]
public static class SerialPortExtensions
{
    [ExcludeFromCodeCoverage]
    public static IObservable<SerialDataReceivedEventArgs> ToObservable(this SerialPort serialPort)
    {
        return Observable
            .FromEvent<SerialDataReceivedEventHandler, SerialDataReceivedEventArgs>(
                handler =>
                {
                    return SdHandler;

                    void SdHandler(object sender, SerialDataReceivedEventArgs args)
                    {
                        handler(args);
                    }
                },
                handler => serialPort.DataReceived += handler,
                handler => serialPort.DataReceived -= handler);
    }

    public static IObservable<SerialDataReceivedEventArgs> ToObservable(this ISerialPort serialPort)
    {
        return Observable
            .FromEvent<SerialDataReceivedEventHandler, SerialDataReceivedEventArgs>(
                handler =>
                {
                    return SdHandler;

                    void SdHandler(object sender, SerialDataReceivedEventArgs args)
                    {
                        handler(args);
                    }
                },
                handler => serialPort.DataReceived += handler,
                handler => serialPort.DataReceived -= handler);
    }

    [ExcludeFromCodeCoverage]
    public static IEnumerable<byte> ReadAllBytes(this SerialPort serialPort)
    {
        var buffer = new byte[serialPort.BytesToRead];

        serialPort.Read(buffer, 0, buffer.Length);

        return buffer;
    }

    [ExcludeFromCodeCoverage]
    public static IObservable<byte[]> ToDataObservable(this SerialPort serialPort)
    {
        return serialPort
            .ToObservable()
            .Select(_ =>
            {
                var data = serialPort.ReadAllBytes().ToArray();

                return data;
            });
    }

    public static IObservable<byte[]> ToDataObservable(this ISerialPort serialPort)
    {
        return serialPort
            .ToObservable()
            .Select(_ =>
            {
                var data = serialPort.ReadAllBytes().ToArray();

                return data;
            });
    }
}
