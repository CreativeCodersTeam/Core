using System.Windows;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.DialogServices.TaskDlg
{
    [PublicAPI]
    public interface ITaskDlgService
    {
        void Show(string message);

        void Show(object ownerViewModel, string message);

        void Show(Window owner, string message);

        void Show(string message, string title);

        void Show(object ownerViewModel, string message, string title);

        void Show(Window owner, string message, string title);

        void Show(TaskDlgIcon taskDlgIcon, string message);

        void Show(TaskDlgIcon taskDlgIcon, object ownerViewModel, string message);

        void Show(TaskDlgIcon taskDlgIcon, Window owner, string message);

        void Show(TaskDlgIcon taskDlgIcon, string message, string title);

        void Show(TaskDlgIcon taskDlgIcon, object ownerViewModel, string message, string title);

        void Show(TaskDlgIcon taskDlgIcon, Window owner, string message, string title);

        TaskDlgResult Show(TaskDlgSettings settings);
    }
}
