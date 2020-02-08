using System;
using System.Security.Principal;
using CreativeCoders.Windows.Api;
using JetBrains.Annotations;
using Microsoft.Win32.SafeHandles;

namespace CreativeCoders.Windows.Security
{
    [PublicAPI]
    public class Impersonation : IDisposable
    {
        private SafeAccessTokenHandle _safeAccessTokenHandle;

        public Impersonation(string domain, string userName, string password)
        {
            CreateSafeAccessTokenHandle(domain, userName, password);
        }

        public void Execute(Action execute)
        {
            WindowsIdentity.RunImpersonated(_safeAccessTokenHandle, execute);
        }

        public T Execute<T>(Func<T> execute)
        {
            return WindowsIdentity.RunImpersonated(_safeAccessTokenHandle, execute);
        }

        private void CreateSafeAccessTokenHandle(string domain, string userName, string password)
        {
            var loginSucceeded = WinApi.LogonUser(userName, domain, password,
                WinApiConstants.LOGON32_LOGON_INTERACTIVE, WinApiConstants.LOGON32_PROVIDER_DEFAULT,
                out _safeAccessTokenHandle);

            if (!loginSucceeded)
            {
                throw new UnauthorizedAccessException("Authorization failed");
            }
        }

        public void Dispose()
        {
            _safeAccessTokenHandle.Dispose();
        }
    }
}