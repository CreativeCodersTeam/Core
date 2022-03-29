using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace CreativeCoders.Windows.Api.User32Dll;

/// <summary>   Static class containing WinApi function imports from User32.dll. </summary>
[ExcludeFromCodeCoverage]
public static class User32Api
{
    private const string User32DllName = "user32.dll";

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Delegate called while enumerating windows. </summary>
    ///
    /// <param name="hWnd">     The window. </param>
    /// <param name="lParam">   The parameter. </param>
    ///
    /// <returns>   A bool. </returns>
    ///-------------------------------------------------------------------------------------------------
    public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   User32.dll SetWindowPos function. See Microsoft WinApi documentation. </summary>
    ///-------------------------------------------------------------------------------------------------
    [DllImport(User32DllName, SetLastError = true)]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, SetWindowPosFlags uFlags);

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   User32.dll EnumChildWindows function. See Microsoft WinApi documentation. </summary>
    ///-------------------------------------------------------------------------------------------------
    [DllImport(User32DllName)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool EnumChildWindows(IntPtr hWndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam);

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   User32.dll GetWindowThreadProcessId function. See Microsoft WinApi documentation. </summary>
    ///-------------------------------------------------------------------------------------------------
    [DllImport(User32DllName)]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpDwProcessId);

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   User32.dll IsWindowVisible function. See Microsoft WinApi documentation. </summary>
    ///-------------------------------------------------------------------------------------------------
    [DllImport(User32DllName)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsWindowVisible(IntPtr hWnd);

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   User32.dll GetWindowText function. See Microsoft WinApi documentation. </summary>
    ///-------------------------------------------------------------------------------------------------
    [DllImport(User32DllName, SetLastError = true)]
    public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   User32.dll LoadString function. See Microsoft WinApi documentation. </summary>
    ///-------------------------------------------------------------------------------------------------
    [DllImport(User32DllName)]
    public static extern int LoadString(IntPtr hInstance, uint uId, StringBuilder lpBuffer, int nBufferMax);
}