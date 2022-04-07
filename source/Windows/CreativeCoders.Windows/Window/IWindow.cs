using System;
using JetBrains.Annotations;

namespace CreativeCoders.Windows.Window;

/// <summary>   Interface for accessing a window. </summary>
[PublicAPI]
public interface IWindow
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Sets if window is top most or not. </summary>
    ///
    /// <param name="isTopMost">    True if is top most, false if not. </param>
    ///-------------------------------------------------------------------------------------------------
    void SetTopMost(bool isTopMost);

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets the title. </summary>
    ///
    /// <value> The title. </value>
    ///-------------------------------------------------------------------------------------------------
    string Title { get; }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets a value indicating whether this window is visible. </summary>
    ///
    /// <value> True if this window is visible, false if not. </value>
    ///-------------------------------------------------------------------------------------------------
    bool IsVisible { get; }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets the identifier of the process. </summary>
    ///
    /// <value> The identifier of the process. </value>
    ///-------------------------------------------------------------------------------------------------
    int ProcessId { get; }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets the Win32 handle of the window. </summary>
    ///
    /// <value> The window handle. </value>
    ///-------------------------------------------------------------------------------------------------
    IntPtr WindowHandle { get; }
}
