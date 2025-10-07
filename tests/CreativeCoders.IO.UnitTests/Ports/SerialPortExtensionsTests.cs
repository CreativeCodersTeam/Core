using System.Diagnostics.CodeAnalysis;
using System.IO.Ports;
using System.Reflection;
using System.Text;
using CreativeCoders.IO.Ports;
using FakeItEasy;
using AwesomeAssertions;

namespace CreativeCoders.IO.UnitTests.Ports;

public class SerialPortExtensionsTests
{
    [Fact]
    public void ToObservable_DataReceivedEventsFired_DataPushedToObservable()
    {
        // Arrange
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

        // Act
        serialPort.DataReceived +=
            Raise.FreeForm<SerialDataReceivedEventHandler>.With(serialPort, eventArgs);

        // Assert
        dataReceived
            .Should()
            .BeTrue();

        eventData
            .Should()
            .Be(eventArgs);
    }

    [Fact]
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public void ToDataObservable_StringDataReceivedEventsFired_StringPushedToObservable()
    {
        // Arrange
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

        // Act
        serialPort.DataReceived +=
            Raise.FreeForm<SerialDataReceivedEventHandler>.With(serialPort, eventArgs);

        // Assert
        eventData
            .Should()
            .NotBeNull();

        var dataText = Encoding.ASCII.GetString(eventData!);

        dataText
            .Should()
            .Be("test");
    }

    [Fact]
    public void ToObservable_DataReceivedEventsFiredAfterSubscriberDispose_NoDataPushedToObservable()
    {
        // Arrange
        var serialPort = A.Fake<ISerialPort>();

        var observable = serialPort.ToObservable();

        var dataReceived = false;
        SerialDataReceivedEventArgs? eventData = null;

        var subscription = observable.Subscribe(data =>
        {
            eventData = data;
            dataReceived = true;
        });

        // Act
        subscription.Dispose();

        var eventArgs = CreateMockedSerialDataReceivedEventArgs();

        serialPort.DataReceived +=
            Raise.FreeForm<SerialDataReceivedEventHandler>.With(serialPort, eventArgs);

        // Assert
        dataReceived
            .Should()
            .BeFalse();

        eventData
            .Should()
            .BeNull();
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
