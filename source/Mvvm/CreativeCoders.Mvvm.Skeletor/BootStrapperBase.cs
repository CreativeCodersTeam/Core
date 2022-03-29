using System;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using CreativeCoders.Di;
using CreativeCoders.Di.Building;
using CreativeCoders.Mvvm.Skeletor.Infrastructure;
using CreativeCoders.Mvvm.Wpf;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Skeletor;

[PublicAPI]
public abstract class BootStrapperBase
{
    private bool _isInitialized;

    protected BootStrapperBase()
    {
        if (GuiDevHelper.IsInDesignMode)
        {
            return;
        }

        AutoShowShellWindow = true;
        Initialize();
    }

    protected void Initialize()
    {
        if (_isInitialized)
        {
            return;
        }
        _isInitialized = true;

        Application = Application.Current;
        InitApplication();
        CreateDiContainer();
        ExecuteConfigure();
    }

    private void ExecuteConfigure()
    {
        var configureMethod = GetType().GetMethod("Configure", BindingFlags.NonPublic | BindingFlags.Instance);
        if (configureMethod != null)
        {
            MethodExecutor.Execute(DiContainer, this, configureMethod);
        }
    }

    protected virtual void InitApplication()
    {
        Application.Startup += OnAppStartup;
        Application.DispatcherUnhandledException += (_, e) => OnUnhandledException(e);
        Application.Exit += OnAppExit;
    }

    private void CreateDiContainer()
    {
        var containerBuilder = CreateDiContainerBuilder();

        ConfigureDiContainer(containerBuilder);

        DiContainer = containerBuilder.Build();
        ServiceLocator.Init(() => DiContainer);
    }

    protected abstract IDiContainerBuilder CreateDiContainerBuilder();

    private void OnAppExit(object sender, ExitEventArgs e)
    {
        OnExit(e);
    }

    private void OnAppStartup(object sender, StartupEventArgs e)
    {
        OnStartUp(e);
        var shell = CreateShell();
        if (shell is FrameworkElement shellFrameworkElement)
        {
            shellFrameworkElement.Loaded += (_, _) => OnShellLoaded();
        }
        // ReSharper disable once InvertIf
        if (AutoShowShellWindow)
        {
            var shellWindow = shell as Window;
            shellWindow?.Show();
        }
    }

    protected virtual void OnShellLoaded() {}

    protected virtual void OnStartUp(StartupEventArgs e) {}

    protected virtual void OnExit(ExitEventArgs e) {}

    protected virtual void OnUnhandledException(DispatcherUnhandledExceptionEventArgs e) {}

    protected virtual void ConfigureDiContainer(IDiContainerBuilder containerBuilder) {}

    protected abstract DependencyObject CreateShell();

    protected Application Application { get; private set; }

    protected bool AutoShowShellWindow { get; set; }

    protected IDiContainer DiContainer { get; private set; }
}

[PublicAPI]
public abstract class BootStrapperBase<TMainViewModel> : BootStrapperBase
    where TMainViewModel : class
{
    private Action<TMainViewModel> _configureMainViewModel;

    protected sealed override DependencyObject CreateShell()
    {
        var mainViewModel = DiContainer.GetInstance<TMainViewModel>();
            
        _configureMainViewModel?.Invoke(mainViewModel);
            
        var mainWindow = DiContainer.GetInstance<IWindowManager>().CreateWindow(mainViewModel);
            
        return mainWindow;
    }

    protected virtual void ConfigureShell(Action<TMainViewModel> configureMainViewModel)
    {
        _configureMainViewModel = configureMainViewModel;
    }
}