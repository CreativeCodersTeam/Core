using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.FileDialogService;

[PublicAPI]
public interface IFileDialogService
{
    bool ShowOpenFileDialog(OpenFileDialogOptions openDialogOptions);

    bool ShowSaveFileDialog(SaveFileDialogOptions saveDialogOptions);
}
