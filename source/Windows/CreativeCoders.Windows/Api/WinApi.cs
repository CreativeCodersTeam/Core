using System;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using Microsoft.Win32.SafeHandles;

namespace CreativeCoders.Windows.Api;

[PublicAPI]
public static class WinApi
{
    private const string Kernel32Dll = "kernel32.dll";

    private const string Advapi32Dll = "advapi32.dll";

    [DllImport(Kernel32Dll, CharSet = CharSet.Auto)]
    public static extern IntPtr FindResourceEx(IntPtr hModule, IntPtr lpType,
        IntPtr lpName, ushort wLanguage);

    [DllImport(Kernel32Dll, SetLastError = true)]
    public static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);

    [DllImport(Kernel32Dll, CharSet = CharSet.Auto)]
    public static extern IntPtr LoadLibrary(string lpFileName);
        
    [DllImport(Advapi32Dll, SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword,
        int dwLogonType, int dwLogonProvider, out SafeAccessTokenHandle phToken);
}