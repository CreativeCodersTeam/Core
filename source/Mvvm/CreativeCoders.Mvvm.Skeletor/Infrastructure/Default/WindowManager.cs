using System.Windows;
using CreativeCoders.Core;
using CreativeCoders.Mvvm.Wpf;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Skeletor.Infrastructure.Default
{
    [PublicAPI]
    public class WindowManager : IWindowManager
    {
        private readonly IViewLocator _viewLocator;

        private readonly IViewModelBinder _viewModelBinder;

        private readonly IWindowHelper _windowHelper;

        public WindowManager(IViewLocator viewLocator, IViewModelBinder viewModelBinder, IWindowHelper windowHelper)
        {
            Ensure.IsNotNull(viewLocator, nameof(viewLocator));
            Ensure.IsNotNull(viewModelBinder, nameof(viewModelBinder));
            Ensure.IsNotNull(windowHelper, nameof(windowHelper));

            _viewLocator = viewLocator;
            _viewModelBinder = viewModelBinder;
            _windowHelper = windowHelper;
        }

        public bool? ShowDialog(object viewModel)
        {
            var window = CreateWindow(viewModel);
            return window.ShowDialog();
        }

        public bool? ShowDialog(object viewModel, object ownerViewModel)
        {
            var window = CreateWindow(viewModel, ownerViewModel);
            return window.ShowDialog();
        }

        public bool? ShowDialog(object viewModel, Window ownerWindow)
        {
            var window = CreateWindow(viewModel, ownerWindow);
            return window.ShowDialog();
        }

        public void ShowWindow(object viewModel)
        {
            var window = CreateWindow(viewModel);
            window.Show();
        }

        public void ShowWindow(object viewModel, object ownerViewModel)
        {
            var window = CreateWindow(viewModel, ownerViewModel);
            window.Show();
        }

        public void ShowWindow(object viewModel, Window ownerWindow)
        {
            var window = CreateWindow(viewModel, ownerWindow);
            window.Show();
        }

        public Window CreateWindow(object viewModel)
        {
            var ownerWindow = Application.Current.MainWindow;
            return CreateWindow(viewModel, ownerWindow);
        }

        public Window CreateWindow(object viewModel, object ownerViewModel)
        {
            var ownerWindow = _windowHelper.FindOwnerWindow(ownerViewModel);
            return CreateWindow(viewModel, ownerWindow);
        }

        public Window CreateWindow(object viewModel, Window ownerWindow)
        {
            var view = _viewLocator.CreateViewForViewModel(viewModel);

            var window = view as Window ?? new Window {Content = view};

            window.Owner = ownerWindow;

            _viewModelBinder.Bind(view, viewModel);
            _viewModelBinder.BindWindow(window, viewModel);

            return window;
        }

        public void CloseWindow(object viewModel)
        {
            var window = _windowHelper.FindOwnerWindow(viewModel);
            window?.Close();
        }

        public void CloseDialog(object viewModel, bool? dialogResult)
        {
            var window = _windowHelper.FindOwnerWindow(viewModel);
            if (window == null)
            {
                return;
            }
            window.DialogResult = dialogResult;
        }
    }
}
