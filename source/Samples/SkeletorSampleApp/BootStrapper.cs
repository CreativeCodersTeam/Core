using CreativeCoders.Mvvm.FileDialogService;
using CreativeCoders.Mvvm.Skeletor;
using CreativeCoders.Mvvm.Skeletor.Infrastructure;
using CreativeCoders.Mvvm.Skeletor.Infrastructure.Default;
using Microsoft.Extensions.DependencyInjection;
using SkeletorSampleApp.ViewModels;

namespace SkeletorSampleApp;

public class BootStrapper : BootStrapperBase<MainViewModel>
{
    protected void Configure(IViewAttributeInitializer viewAttributeInitializer)
    {
        viewAttributeInitializer.InitFromAssembly(typeof(BootStrapper).Assembly);
        ConfigureShell(x => x.Title ="Sample demo app for Skeletor Framework");
    }
        
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<FirstDetailsViewModel>();
        services.AddSingleton<SecondDetailsViewModel>();

        services.SetupDefaultInfrastructure();

        services.AddTransient<IFileDialogService, Win32FileDialogService>();
    }
}
