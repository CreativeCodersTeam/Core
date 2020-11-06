using System;
using JetBrains.Annotations;

namespace CreativeCoders.Windows.Api.User32Dll
{
    [PublicAPI]
    public static class HWnd
    {
        public static IntPtr NoTopMost = new IntPtr(-2);

        public static IntPtr TopMost = new IntPtr(-1);

        public static IntPtr Top = new IntPtr(0);

        public static IntPtr Bottom = new IntPtr(1);
    }
}