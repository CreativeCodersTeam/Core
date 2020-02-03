using System.Text;
using CreativeCoders.Windows.Api;
using JetBrains.Annotations;

namespace CreativeCoders.Windows
{
    [PublicAPI]
    public static class WindowsButtonUtils
    {
        public static string GetCaptionText(WindowsButtonCaption buttonCaption)
        {
            var handle = WinApi.LoadLibrary("user32.dll");

            var sb = new StringBuilder(1024);

            return WinApi.LoadString(handle, (uint)buttonCaption, sb, 1024) > 0 ? sb.ToString() : string.Empty;
        }

        public static string GetCaptionText(WindowsButtonCaption buttonCaption, char shortCutUnderlineChar)
        {
            return GetCaptionText(buttonCaption).Replace('&', shortCutUnderlineChar);
        }

        public static string GetWpfCaptionText(WindowsButtonCaption buttonCaption)
        {
            return GetCaptionText(buttonCaption, '_');
        }
    }
}