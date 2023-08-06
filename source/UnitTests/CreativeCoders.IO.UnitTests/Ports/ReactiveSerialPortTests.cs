using System.IO.Ports;
using System.Reflection;
using System.Text;
using CreativeCoders.IO.Ports;
using FakeItEasy;
using FluentAssertions;

namespace CreativeCoders.IO.UnitTests.Ports;

public class ReactiveSerialPortTests
{
    [Fact]
    public void Open_CallsSerialPortOpen()
    {
        // Arrange
        var serialPort = A.Fake<ISerialPort>();
        var reactiveSerialPort = new ReactiveSerialPort(serialPort);

        // Act
        reactiveSerialPort.Open();

        // Assert
        A.CallTo(() => serialPort.Open()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void Close_CallsSerialPortClose()
    {
        // Arrange
        var serialPort = A.Fake<ISerialPort>();
        var reactiveSerialPort = new ReactiveSerialPort(serialPort);

        // Act
        reactiveSerialPort.Close();

        // Assert
        A.CallTo(() => serialPort.Close()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void Subscribe_CallsDataObservableSubscribe()
    {
        var serialPort = A.Fake<ISerialPort>();
        var reactiveSerialPort = new ReactiveSerialPort(serialPort);

        var dataReceived = false;
        byte[]? eventData = null;

        var buffer0 = Encoding.ASCII.GetBytes("test");

        A
            .CallTo(() => serialPort.ReadAllBytes())
            .Returns(buffer0);

        reactiveSerialPort.Subscribe(data =>
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

        var dataText = Encoding.ASCII.GetString(eventData!);

        dataText
            .Should()
            .Be("test");
        // eventData
        //     .Should()
        //     .NotBeNull();
    }

    [Fact]
    public void Dispose_CallsSerialPortDispose()
    {
        // Arrange
        var serialPort = A.Fake<ISerialPort>();
        var reactiveSerialPort = new ReactiveSerialPort(serialPort);

        // Act
        reactiveSerialPort.Dispose();

        // Assert
        A.CallTo(() => serialPort.Dispose()).MustHaveHappenedOnceExactly();
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
