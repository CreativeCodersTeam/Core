﻿using System;

namespace CreativeCoders.SysConsole.CliArguments.Parsing.Properties
{
    public interface ICliValueConverter
    {
        object? Convert(object? value, Type targetType);
    }
}