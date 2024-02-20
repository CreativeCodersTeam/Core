using CreativeCoders.Mvvm.Skeletor.Infrastructure;
using JetBrains.Annotations;
using SkeletorSampleApp.ViewModels;

namespace SkeletorSampleApp.Views;

[UsedImplicitly]
[View(typeof(MainViewModel))]
public partial class MainView
{
    public MainView()
    {
        InitializeComponent();
    }
}
