using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace CreativeCoders.Windows.Api
{
    public static class WinApi
    {
        private const string User32Dll = "user32.dll";

        private const string Kernel32Dll = "kernel32.dll";

        private const string Advapi32Dll = "advapi32.dll";
        
        [DllImport(User32Dll, CharSet = CharSet.Auto)]
        public static extern int LoadString(IntPtr hInstance, uint uId, StringBuilder lpBuffer, int nBufferMax);

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
}