using System.Globalization;
using JetBrains.Annotations;

namespace CreativeCoders.Windows.Api
{
    [PublicAPI]
    public class WinApiUtils
    {
        public static ushort MakeLangId(CultureInfo cultureInfo)
        {
            return (ushort)MakeLangId(PrimaryLangId(cultureInfo.LCID), SubLangId(cultureInfo.LCID));
        }

        public static int MakeLangId(int primary, int sub)
        {
            return ((ushort)sub << 10) | (ushort)primary;
        }

        private static int PrimaryLangId(int lcid)
        {
            return (ushort)lcid & 0x3ff;
        }

        private static int SubLangId(int lcid)
        {
            return (ushort)lcid >> 10;
        }
    }
}