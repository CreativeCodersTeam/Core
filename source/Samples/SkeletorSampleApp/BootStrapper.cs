using CreativeCoders.Di.Building;
using CreativeCoders.Di.SimpleInjector;
using CreativeCoders.Mvvm.FileDialogService;
using CreativeCoders.Mvvm.Skeletor;
using CreativeCoders.Mvvm.Skeletor.Infrastructure;
using CreativeCoders.Mvvm.Skeletor.Infrastructure.Default;
using SimpleInjector;
using SkeletorSampleApp.ViewModels;

namespace SkeletorSampleApp;

public class BootStrapper : BootStrapperBase<MainViewModel>
{
    protected override IDiContainerBuilder CreateDiContainerBuilder()
    {
        return new SimpleInjectorDiContainerBuilder(new Container());
    }

    protected void Configure(IViewAttributeInitializer viewAttributeInitializer)
    {
        viewAttributeInitializer.InitFromAllAssemblies();
        ConfigureShell(x => x.Title ="Sample demo app for Skeletor Framework");
    }
        
    protected override void ConfigureDiContainer(IDiContainerBuilder containerBuilder)
    {
        containerBuilder.SetupDefaultInfrastructure();
        containerBuilder.AddTransient<IFileDialogService, Win32FileDialogService>();
    }
}