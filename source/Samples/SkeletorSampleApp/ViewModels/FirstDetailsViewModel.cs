using CreativeCoders.Mvvm;
using CreativeCoders.Mvvm.Commands;
using CreativeCoders.Mvvm.FileDialogService;
using JetBrains.Annotations;

namespace SkeletorSampleApp.ViewModels
{
    [PublicAPI]
    public class FirstDetailsViewModel : ViewModelBase
    {
        internal FirstDetailsViewModel()
        {
        }
        
        public FirstDetailsViewModel(IFileDialogService fileDialogService)
        {
            OpenFileDialogCommand = new SimpleRelayCommand(() => ExecuteOpenFileDialog(fileDialogService));
        }

        private static void ExecuteOpenFileDialog(IFileDialogService fileDialogService)
        {
            fileDialogService.ShowOpenFileDialog(new OpenFileDialogOptions());
        }

        public ICommandEx OpenFileDialogCommand { get; }
    }
}