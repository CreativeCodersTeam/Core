using System.Globalization;
using JetBrains.Annotations;

namespace CreativeCoders.Windows.Api;

/// <summary>   Misc methods for working with LCID. </summary>
[PublicAPI]
public class WinApiUtils
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Makes language identifier for a culture info. </summary>
    ///
    /// <param name="cultureInfo">  Information describing the culture. </param>
    ///
    /// <returns>   The LCID. </returns>
    ///-------------------------------------------------------------------------------------------------
    public static ushort MakeLangId(CultureInfo cultureInfo)
    {
        return (ushort)MakeLangId(PrimaryLangId(cultureInfo.LCID), SubLangId(cultureInfo.LCID));
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Makes language identifier for a culture info. </summary>
    ///
    /// <param name="primary">  The primary language id. </param>
    /// <param name="sub">      The sub language id. </param>
    ///
    /// <returns>   The LCID. </returns>
    ///-------------------------------------------------------------------------------------------------
    public static int MakeLangId(int primary, int sub)
    {
        return ((ushort)sub << 10) | (ushort)primary;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets the primary language identifier for a LCID. </summary>
    ///
    /// <param name="lcid"> The LCID. </param>
    ///
    /// <returns>   The primary language id. </returns>
    ///-------------------------------------------------------------------------------------------------
    private static int PrimaryLangId(int lcid)
    {
        return (ushort)lcid & 0x3ff;
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Gets the sub language identifier for a LCID. </summary>
    ///
    /// <param name="lcid"> The LCID. </param>
    ///
    /// <returns>   The sub language id. </returns>
    ///-------------------------------------------------------------------------------------------------
    private static int SubLangId(int lcid)
    {
        return (ushort)lcid >> 10;
    }
}