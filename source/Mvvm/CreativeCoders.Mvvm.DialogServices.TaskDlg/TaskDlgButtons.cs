using System;

namespace CreativeCoders.Mvvm.DialogServices.TaskDlg
{
    [Flags]
    public enum TaskDlgButtons
    {
        None = 0,
        Ok = 1,
        Yes = 2,
        No = 4,
        Retry = 8,
        Cancel = 16,
        Close = 32
    }
}
