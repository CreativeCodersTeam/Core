using System;
using System.Windows;
using System.Windows.Forms;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.DialogServices.TaskDialog;

[PublicAPI]
public interface ITaskDialog
{
    Window GetOwnerForViewModel(object viewModel);

    TaskDialogButton ShowDialog(TaskDialogPage page,
        TaskDialogStartupLocation startupLocation = TaskDialogStartupLocation.CenterOwner);

    TaskDialogButton ShowDialog(IntPtr hwndOwner, TaskDialogPage page,
        TaskDialogStartupLocation startupLocation = TaskDialogStartupLocation.CenterOwner);

    TaskDialogButton ShowDialog(IWin32Window owner, TaskDialogPage page,
        TaskDialogStartupLocation startupLocation = TaskDialogStartupLocation.CenterOwner);
}
