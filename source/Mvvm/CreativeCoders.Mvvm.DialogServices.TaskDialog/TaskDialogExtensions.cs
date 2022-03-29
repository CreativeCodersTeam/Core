using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using JetBrains.Annotations;
using IWin32Window = System.Windows.Forms.IWin32Window;

namespace CreativeCoders.Mvvm.DialogServices.TaskDialog;

[PublicAPI]
public static class TaskDialogExtensions
{
    public static void ShowDialog(this ITaskDialog taskDialog, string text, string caption = null)
    {
        taskDialog.ShowDialog(
            new TaskDialogPage
            {
                Caption = caption,
                Text = text
            });
    }

    public static void ShowDialog(this ITaskDialog taskDialog, object ownerViewModel, string text,
        string caption = null)
    {
        taskDialog.ShowDialog(taskDialog.GetOwnerForViewModel(ownerViewModel), text, caption);
    }

    public static void ShowDialog(this ITaskDialog taskDialog, Window owner, string text,
        string caption = null)
    {
        taskDialog.ShowDialog(new WindowInteropHelper(owner).Handle, text, caption);
    }

    public static void ShowDialog(this ITaskDialog taskDialog, IWin32Window owner, string text,
        string caption = null)
    {
        taskDialog.ShowDialog(owner,
            new TaskDialogPage
            {
                Caption = caption,
                Text = text
            });
    }

    public static void ShowDialog(this ITaskDialog taskDialog, IntPtr ownerWindowHandle, string text,
        string caption = null)
    {
        taskDialog.ShowDialog(ownerWindowHandle,
            new TaskDialogPage
            {
                Caption = caption,
                Text = text
            });
    }

    public static void ShowDialog(this ITaskDialog taskDialog, TaskDialogIcon taskDialogIcon, string text,
        string caption = null)
    {
        taskDialog.ShowDialog(
            new TaskDialogPage
            {
                Icon = taskDialogIcon,
                Caption = caption,
                Text = text
            });
    }

    public static void ShowDialog(this ITaskDialog taskDialog, TaskDialogIcon taskDialogIcon,
        object ownerViewModel,
        string text, string caption = null)
    {
        taskDialog.ShowDialog(taskDialogIcon, taskDialog.GetOwnerForViewModel(ownerViewModel), text, caption);
    }

    public static void ShowDialog(this ITaskDialog taskDialog, TaskDialogIcon taskDialogIcon, Window owner,
        string text, string caption = null)
    {
        taskDialog.ShowDialog(taskDialogIcon, new WindowInteropHelper(owner).Handle, text, caption);
    }

    public static void ShowDialog(this ITaskDialog taskDialog, TaskDialogIcon taskDialogIcon,
        IWin32Window owner,
        string text, string caption = null)
    {
        taskDialog.ShowDialog(owner,
            new TaskDialogPage
            {
                Icon = taskDialogIcon,
                Caption = caption,
                Text = text
            });
    }

    public static void ShowDialog(this ITaskDialog taskDialog, TaskDialogIcon taskDialogIcon,
        IntPtr hwndOwner,
        string text, string caption = null)
    {
        taskDialog.ShowDialog(hwndOwner,
            new TaskDialogPage
            {
                Icon = taskDialogIcon,
                Caption = caption,
                Text = text
            });
    }
}
