using System;
using JetBrains.Annotations;

namespace CreativeCoders.Windows.Api.User32Dll;

[PublicAPI]
public static class HWnd
{
    public static IntPtr NoTopMost { get; } = new(-2);

    public static IntPtr TopMost { get; } = new(-1);

    public static IntPtr Top { get; } = new(0);

    public static IntPtr Bottom { get; } = new(1);
}
