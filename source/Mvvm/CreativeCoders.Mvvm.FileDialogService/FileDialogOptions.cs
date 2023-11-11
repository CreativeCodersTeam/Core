using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.FileDialogService;

[PublicAPI]
public class FileDialogOptions
{
    public bool AddExtension { get; set; } = true;

    public bool CheckFileExists { get; set; }

    public bool CheckPathExists { get; set; } = true;

    public string DefaultExt { get; set; }

    public bool DereferenceLinks { get; set; }

    public string FileName { get; set; }

    public string[] FileNames { get; set; }

    public string Filter { get; set; }

    public int FilterIndex { get; set; } = 1;

    public string InitialDirectory { get; set; }

    public bool RestoreDirectory { get; set; }

    public string Title { get; set; }

    public bool ValidateNames { get; set; }
}
