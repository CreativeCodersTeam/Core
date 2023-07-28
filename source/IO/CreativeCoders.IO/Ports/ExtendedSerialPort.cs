﻿using System.IO.Ports;

namespace CreativeCoders.IO.Ports;

public class ExtendedSerialPort : SerialPort, ISerialPort
{
    public ExtendedSerialPort(string portName) : base(portName)
    {

    }

    public IEnumerable<byte> ReadAllBytes()
    {
        return SerialPortExtensions.ReadAllBytes(this);
    }
}
