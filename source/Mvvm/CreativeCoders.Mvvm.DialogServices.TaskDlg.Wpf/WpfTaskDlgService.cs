using System.Windows;
using CreativeCoders.Core;
using CreativeCoders.Mvvm.Wpf;
using JetBrains.Annotations;
using TaskDialogInterop;

namespace CreativeCoders.Mvvm.DialogServices.TaskDlg.Wpf
{
    [PublicAPI]
    public class WpfTaskDlgService : ITaskDlgService
    {
        private readonly IWindowHelper _windowHelper;

        public WpfTaskDlgService(IWindowHelper windowHelper)
        {
            Ensure.IsNotNull(windowHelper, nameof(windowHelper));

            _windowHelper = windowHelper;
        }

        static WpfTaskDlgService()
        {
            TaskDialog.ForceEmulationMode = true;
        }

        public void Show(string message)
        {
            Show(_windowHelper.GetActiveWindow(), message);
        }

        public void Show(object ownerViewModel, string message)
        {
            Show(GetOwner(ownerViewModel), message);
        }

        public void Show(Window owner, string message)
        {
            TaskDialog.ShowMessage(owner, message);
        }

        public void Show(string message, string title)
        {
            Show(_windowHelper.GetActiveWindow(), message, title);
        }

        public void Show(object ownerViewModel, string message, string title)
        {
            Show(GetOwner(ownerViewModel), message, title);
        }

        public void Show(Window owner, string message, string title)
        {
            Show(TaskDlgIcon.None, owner, message, title);
        }

        public void Show(TaskDlgIcon taskDlgIcon, string message)
        {
            Show(taskDlgIcon, _windowHelper.GetActiveWindow(), message);
        }

        public void Show(TaskDlgIcon taskDlgIcon, object ownerViewModel, string message)
        {
            Show(taskDlgIcon, GetOwner(ownerViewModel), message);
        }

        public void Show(TaskDlgIcon taskDlgIcon, Window owner, string message)
        {
            var options = TaskDialogOptions.Default;
            options.Owner = owner;
            options.Content = message;
            options.CustomButtons = WpfTaskDialogHelper.ToCustomButtons(TaskDlgButtons.Ok);
            options.MainIcon = WpfTaskDialogHelper.ToVistaTaskDialogIcon(taskDlgIcon);
            TaskDialog.Show(options);
        }

        public void Show(TaskDlgIcon taskDlgIcon, string message, string title)
        {
            Show(taskDlgIcon, _windowHelper.GetActiveWindow(), message, title);
        }

        public void Show(TaskDlgIcon taskDlgIcon, object ownerViewModel, string message, string title)
        {
            Show(taskDlgIcon, GetOwner(ownerViewModel), message, title);
        }

        public void Show(TaskDlgIcon taskDlgIcon, Window owner, string message, string title)
        {
            var options = TaskDialogOptions.Default;
            options.Owner = owner;
            options.Title = title;
            options.Content = message;
            options.CustomButtons = WpfTaskDialogHelper.ToCustomButtons(TaskDlgButtons.Ok);
            options.MainIcon = WpfTaskDialogHelper.ToVistaTaskDialogIcon(taskDlgIcon);
            TaskDialog.Show(options);
        }

        public TaskDlgResult Show(TaskDlgSettings settings)
        {
            var options = WpfTaskDialogHelper.ToTaskDialogOptions(settings);
            if (options.Owner == null)
            {
                options.Owner = GetOwner(null);
            }
            var result = TaskDialog.Show(options);
            return WpfTaskDialogHelper.ToTaskDlgResult(result, settings);
        }

        private Window GetOwner(object viewModel)
        {
            return viewModel == null ? _windowHelper.GetActiveWindow() : _windowHelper.FindOwnerWindow(viewModel) ?? Application.Current.MainWindow;
        }
    }
}
