using CreativeCoders.Windows;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.DialogServices.TaskDlg.Wpf
{
    [PublicAPI]
    public static class TaskDlgButtonCaptions
    {
        public static string Ok { get; set; } = WindowsButtonUtils.GetWpfCaptionText(WindowsButtonCaption.Ok);

        public static string Yes { get; set; } = WindowsButtonUtils.GetWpfCaptionText(WindowsButtonCaption.Yes);

        public static string No { get; set; } = WindowsButtonUtils.GetWpfCaptionText(WindowsButtonCaption.No);

        public static string Retry { get; set; } = WindowsButtonUtils.GetWpfCaptionText(WindowsButtonCaption.Retry);

        public static string Cancel { get; set; } = WindowsButtonUtils.GetWpfCaptionText(WindowsButtonCaption.Cancel);

        public static string Close { get; set; } = WindowsButtonUtils.GetWpfCaptionText(WindowsButtonCaption.Close);
    }
}