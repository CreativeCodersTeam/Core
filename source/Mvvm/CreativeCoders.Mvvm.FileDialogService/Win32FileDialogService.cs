using AutoMapper;
using CreativeCoders.Core;
using JetBrains.Annotations;
using Microsoft.Win32;

namespace CreativeCoders.Mvvm.FileDialogService;

[PublicAPI]
public class Win32FileDialogService : IFileDialogService
{
    private static readonly MapperConfiguration MapperConfig;

    static Win32FileDialogService()
    {
        MapperConfig = new MapperConfiguration(CreateMaps);
    }

    private static void CreateMaps(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<OpenFileDialogOptions, OpenFileDialog>();
        cfg.CreateMap<SaveFileDialogOptions, SaveFileDialog>();

        cfg.CreateMap<OpenFileDialog, OpenFileDialogOptions>();
        cfg.CreateMap<SaveFileDialog, SaveFileDialogOptions>();
    }

    bool IFileDialogService.ShowOpenFileDialog(OpenFileDialogOptions openDialogOptions)
    {
        Ensure.IsNotNull(openDialogOptions, nameof(openDialogOptions));

        var openFileDlg = new OpenFileDialog();

        var mapper = MapperConfig.CreateMapper();
        mapper.Map(openDialogOptions, openFileDlg);

        var dialogResult = openFileDlg.ShowDialog() == true;

        mapper.Map(openFileDlg, openDialogOptions);

        return dialogResult;
    }

    bool IFileDialogService.ShowSaveFileDialog(SaveFileDialogOptions saveDialogOptions)
    {
        Ensure.IsNotNull(saveDialogOptions, nameof(saveDialogOptions));

        var saveFileDlg = new SaveFileDialog();

        var mapper = MapperConfig.CreateMapper();
        mapper.Map(saveDialogOptions, saveFileDlg);

        var dialogResult = saveFileDlg.ShowDialog() == true;

        mapper.Map(saveFileDlg, saveDialogOptions);

        return dialogResult;
    }
}
