using System;
using System.Windows;
using System.Windows.Forms;
using CreativeCoders.Core;
using CreativeCoders.Mvvm.Wpf;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.DialogServices.TaskDialog.Default
{
    [UsedImplicitly]
    public class DefaultTaskDialog : ITaskDialog
    {
        private readonly IWindowHelper _windowHelper;

        public DefaultTaskDialog(IWindowHelper windowHelper)
        {
            Ensure.IsNotNull(windowHelper, nameof(windowHelper));

            _windowHelper = windowHelper;
        }

        public Window GetOwnerForViewModel(object viewModel)
        {
            return viewModel == null
                ? _windowHelper.GetActiveWindow()
                : _windowHelper.FindOwnerWindow(viewModel) ?? System.Windows.Application.Current.MainWindow;
        }

        public TaskDialogButton ShowDialog(TaskDialogPage page,
            TaskDialogStartupLocation startupLocation = TaskDialogStartupLocation.CenterOwner)
        {
            return System.Windows.Forms.TaskDialog.ShowDialog(page, startupLocation);
        }

        public TaskDialogButton ShowDialog(IntPtr hwndOwner, TaskDialogPage page,
            TaskDialogStartupLocation startupLocation = TaskDialogStartupLocation.CenterOwner)
        {
            return System.Windows.Forms.TaskDialog.ShowDialog(hwndOwner, page, startupLocation);
        }

        public TaskDialogButton ShowDialog(IWin32Window owner, TaskDialogPage page,
            TaskDialogStartupLocation startupLocation = TaskDialogStartupLocation.CenterOwner)
        {
            return System.Windows.Forms.TaskDialog.ShowDialog(owner, page, startupLocation);
        }
    }
}
