﻿using System;
using CreativeCoders.Core.Collections;
using CreativeCoders.Windows.Window;

namespace WindowsSampleApp;

public static class Program
{
    public static void Main()
    {
        var win32Windows = new Win32Windows();

        var windows = win32Windows.EnumerateWindows();

        windows.ForEach(x => Console.WriteLine($"{x.WindowHandle}: {x.Title}"));

        Console.WriteLine("Press key to exit...");
        Console.ReadKey();
    }
}
