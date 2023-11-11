using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.FileDialogService;

[PublicAPI]
public class SaveFileDialogOptions : FileDialogOptions
{
    public bool CreatePrompt { get; set; }

    public bool OverwritePrompt { get; set; } = true;
}
