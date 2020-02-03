using System.Drawing;
using System.Windows;

namespace CreativeCoders.Mvvm.DialogServices.TaskDlg
{
    public struct TaskDlgSettings
    {
        public TaskDlgIcon MainIcon { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public Window Owner { get; set; }

        public TaskDlgButtons CommonButtons { get; set; }

        public string[] CustomButtons { get; set; }

        public string[] CommandButtons { get; set; }

        public string[] RadioButtons { get; set; }

        public string MainInstruction { get; set; }

        public string ExpandedInfo { get; set; }

        public string FooterText { get; set; }

        public string VerificationText { get; set; }

        public int? DefaultButtonIndex { get; set; }

        public Icon CustomMainIcon { get; set; }

        public bool ExpandedByDefault { get; set; }

        public bool ExpandToFooter { get; set; }

        public bool VerificationByDefault { get; set; }

        public TaskDlgIcon FooterIcon { get; set; }

        public Icon CustomFooterIcon { get; set; }
    }
}
