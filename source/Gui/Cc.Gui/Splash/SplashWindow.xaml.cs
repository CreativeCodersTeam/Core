using System.Windows;

namespace Cc.Gui.Splash
{
    /// <summary>
    /// Interaktionslogik für SplashWindow.xaml
    /// </summary>
    public partial class SplashWindow : Window
    {
        public SplashWindow(SplashViewModel splashViewModel)
        {
            InitializeComponent();
            DataContext = splashViewModel;
        }
    }
}