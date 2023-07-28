using System.IO.Ports;
using System.Reflection;
using System.Text;
using CreativeCoders.IO.Ports;
using FakeItEasy;
using FluentAssertions;

namespace CreativeCoders.IO.UnitTests.Ports;

public class SerialPortExtensionsTests
{
    [Fact]
    public void ToObservable_DataReceivedEventsFired_DataPushedToObservable()
    {
        var serialPort = A.Fake<ISerialPort>();

        var observable = serialPort.ToObservable();

        var dataReceived = false;
        SerialDataReceivedEventArgs? eventData = null;

        observable.Subscribe(data =>
        {
            eventData = data;
            dataReceived = true;
        });

        var eventArgs = CreateMockedSerialDataReceivedEventArgs();

        serialPort.DataReceived +=
            Raise.FreeForm<SerialDataReceivedEventHandler>.With(serialPort, eventArgs);

        dataReceived
            .Should()
            .BeTrue();

        eventData
            .Should()
            .Be(eventArgs);
    }

    [Fact]
    public void ToDataObservable()
    {
        var serialPort = A.Fake<ISerialPort>();

        var observable = serialPort.ToDataObservable();

        var buffer0 = Encoding.ASCII.GetBytes("test");

        A
            .CallTo(() => serialPort.ReadAllBytes())
            .Returns(buffer0);

        byte[]? eventData = null;

        observable.Subscribe(data =>
        {
            eventData = data;
        });

        var eventArgs = CreateMockedSerialDataReceivedEventArgs();

        serialPort.DataReceived +=
            Raise.FreeForm<SerialDataReceivedEventHandler>.With(serialPort, eventArgs);

        eventData
            .Should()
            .NotBeNull();

        var dataText = Encoding.ASCII.GetString(eventData!);

        dataText
            .Should()
            .Be("test");
    }

    private static SerialDataReceivedEventArgs CreateMockedSerialDataReceivedEventArgs()
    {
        var constructor = typeof (SerialDataReceivedEventArgs).GetConstructor(
            BindingFlags.NonPublic | BindingFlags.Instance,
            null,
            new[] {typeof (SerialData)},
            null);

        var eventArgs =
            (SerialDataReceivedEventArgs?)constructor?.Invoke(new object[] {SerialData.Eof});

        return eventArgs ?? throw new InvalidOperationException();
    }
}
