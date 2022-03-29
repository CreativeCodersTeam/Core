using System.Collections.Generic;
using JetBrains.Annotations;

namespace CreativeCoders.Windows.Window;

/// <summary>   Interface for Win32 windows functions. </summary>
[PublicAPI]
public interface IWin32Windows
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Enumerates the windows for a process. </summary>
    ///
    /// <param name="processId">    Identifier for the process. </param>
    ///
    /// <returns>
    ///     An enumerator that allows foreach to be used to process the windows for process in this
    ///     collection.
    /// </returns>
    ///-------------------------------------------------------------------------------------------------
    IEnumerable<IWindow> EnumerateWindowsForProcess(int processId);

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Enumerates the windows. </summary>
    ///
    /// <returns>
    ///     An enumerator that allows foreach to be used to process the windows in this collection.
    /// </returns>
    ///-------------------------------------------------------------------------------------------------
    IEnumerable<IWindow> EnumerateWindows();
}