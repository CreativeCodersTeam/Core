using System.Windows;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Skeletor.Infrastructure
{
    [PublicAPI]
    public interface IWindowManager
    {
        bool? ShowDialog(object viewModel);

        bool? ShowDialog(object viewModel, object ownerViewModel);

        bool? ShowDialog(object viewModel, Window ownerWindow);

        void ShowWindow(object viewModel);

        void ShowWindow(object viewModel, object ownerViewModel);

        void ShowWindow(object viewModel, Window ownerWindow);

        Window CreateWindow(object viewModel);

        Window CreateWindow(object viewModel, object ownerViewModel);

        Window CreateWindow(object viewModel, Window ownerWindow);

        void CloseWindow(object viewModel);

        void CloseDialog(object viewModel, bool? dialogResult);
    }
}
