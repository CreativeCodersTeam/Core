using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.FileDialogService;

[PublicAPI]
public class OpenFileDialogOptions : FileDialogOptions
{
    public OpenFileDialogOptions()
    {
        CheckFileExists = true;
    }

    public bool Multiselect { get; set; }

    public bool ReadOnlyChecked { get; set; }

    public bool ShowReadOnly { get; set; }
}
