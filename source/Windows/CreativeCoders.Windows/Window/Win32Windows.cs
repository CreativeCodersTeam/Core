using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CreativeCoders.Windows.Api.User32Dll;

namespace CreativeCoders.Windows.Window;

///<inheritdoc/>
[ExcludeFromCodeCoverage]
public class Win32Windows : IWin32Windows
{
    public IEnumerable<IWindow> EnumerateWindowsForProcess(int processId)
    {
        return EnumerateWindows().Where(window => window.ProcessId == processId);
    }

    public IEnumerable<IWindow> EnumerateWindows()
    {
        var windows = new List<IWindow>();

        User32Api.EnumChildWindows(IntPtr.Zero, (hWnd, _) =>
        {
            windows.Add(new Win32Window(hWnd));

            return true;
        }, IntPtr.Zero);

        return windows;
    }
}
