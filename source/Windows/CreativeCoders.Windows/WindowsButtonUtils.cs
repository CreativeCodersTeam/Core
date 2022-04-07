using System.Text;
using CreativeCoders.Windows.Api;
using CreativeCoders.Windows.Api.User32Dll;
using JetBrains.Annotations;

namespace CreativeCoders.Windows;

/// <summary>   Class with different methods for working with buttons. </summary>
[PublicAPI]
public static class WindowsButtonUtils
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets caption text for the specified button. </summary>
    ///
    /// <param name="buttonCaption">    The button caption. </param>
    ///
    /// <returns>   The caption text. </returns>
    ///-------------------------------------------------------------------------------------------------
    public static string GetCaptionText(WindowsButtonCaption buttonCaption)
    {
        var handle = WinApi.LoadLibrary("user32.dll");

        var sb = new StringBuilder(1024);

        return User32Api.LoadString(handle, (uint) buttonCaption, sb, 1024) > 0
            ? sb.ToString()
            : string.Empty;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets caption text for the specified button. </summary>
    ///
    /// <param name="buttonCaption">            The button caption. </param>
    /// <param name="shortCutUnderlineChar">    The short cut underline character. </param>
    ///
    /// <returns>   The caption text. </returns>
    ///-------------------------------------------------------------------------------------------------
    public static string GetCaptionText(WindowsButtonCaption buttonCaption, char shortCutUnderlineChar)
    {
        return GetCaptionText(buttonCaption).Replace('&', shortCutUnderlineChar);
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Gets caption text for the specified button with the WPF short cut underline char.
    /// </summary>
    ///
    /// <param name="buttonCaption">    The button caption. </param>
    ///
    /// <returns>   The WPF caption text. </returns>
    ///-------------------------------------------------------------------------------------------------
    public static string GetWpfCaptionText(WindowsButtonCaption buttonCaption)
    {
        return GetCaptionText(buttonCaption, '_');
    }
}
