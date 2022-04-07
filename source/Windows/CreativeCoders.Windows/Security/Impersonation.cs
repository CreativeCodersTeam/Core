using System;
using System.Security.Principal;
using CreativeCoders.Windows.Api;
using JetBrains.Annotations;
using Microsoft.Win32.SafeHandles;

namespace CreativeCoders.Windows.Security;

///-------------------------------------------------------------------------------------------------
/// <summary>   Used to work with impersonation. </summary>
///
/// <seealso cref="IDisposable"/>
///-------------------------------------------------------------------------------------------------
[PublicAPI]
public class Impersonation : IDisposable
{
    private SafeAccessTokenHandle _safeAccessTokenHandle;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Initializes a new instance of the CreativeCoders.Windows.Security.Impersonation class.
    /// </summary>
    ///
    /// <param name="domain">   The domain. </param>
    /// <param name="userName"> Name of the user. </param>
    /// <param name="password"> The password. </param>
    ///-------------------------------------------------------------------------------------------------
    public Impersonation(string domain, string userName, string password)
    {
        CreateSafeAccessTokenHandle(domain, userName, password);
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Executes the given action in the context of the impersonated user. </summary>
    ///
    /// <param name="execute">  The action to execute. </param>
    ///-------------------------------------------------------------------------------------------------
    public void Execute(Action execute)
    {
        WindowsIdentity.RunImpersonated(_safeAccessTokenHandle, execute);
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Executes the given action in the context of the impersonated user. </summary>
    ///
    /// <typeparam name="T">    Generic type parameter. </typeparam>
    /// <param name="execute">  The action to execute. </param>
    ///
    /// <returns>   The result of the function. </returns>
    ///-------------------------------------------------------------------------------------------------
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

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Releases the user context.
    /// </summary>
    ///
    /// <seealso cref="IDisposable.Dispose()"/>
    ///-------------------------------------------------------------------------------------------------
    public void Dispose()
    {
        _safeAccessTokenHandle.Dispose();
    }
}
