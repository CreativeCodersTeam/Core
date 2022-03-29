using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using CreativeCoders.Windows.Api.User32Dll;

namespace CreativeCoders.Windows.Window;

[ExcludeFromCodeCoverage]
internal class Win32Window : IWindow
{
    public Win32Window(IntPtr windowHandle)
    {
        WindowHandle = windowHandle;
    }

    public void SetTopMost(bool isTopMost)
    {
        var topMostHandle = isTopMost ? HWnd.TopMost : HWnd.NoTopMost;

        User32Api.SetWindowPos(WindowHandle, topMostHandle, 0, 0, 0, 0,
            SetWindowPosFlags.IgnoreMove | SetWindowPosFlags.IgnoreResize);
    }

    public string Title
    {
        get
        {
            var sb = new StringBuilder(255);

            User32Api.GetWindowText(WindowHandle, sb, sb.Capacity);

            return sb.ToString();
        }
    }

    public bool IsVisible => User32Api.IsWindowVisible(WindowHandle);

    public int ProcessId
    {
        get
        {
            User32Api.GetWindowThreadProcessId(WindowHandle, out var processId);
            return (int) processId;
        }
    }

    public IntPtr WindowHandle { get; }
}