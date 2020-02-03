using System.Windows;
using CreativeCoders.Mvvm.Ribbon.FluentRibbon;
using JetBrains.Annotations;

namespace RibbonSampleApp
{
    
    [PublicAPI]
    // ReSharper disable once RedundantExtendsListEntry
    public partial class MainWindow : Fluent.RibbonWindow
    {
        public MainWindow()
        {
            FirstTab = new RibbonTabViewModel
            {
                Text = "First Tab"
            };
            DataContext = this;
            InitializeComponent();
        }

        public RibbonTabViewModel FirstTab { get; }

        private void ChangeTab_OnClick(object sender, RoutedEventArgs e)
        {
            FirstTab.Text = "First Text";
            FirstTab.IsVisible = !FirstTab.IsVisible;
            FirstTab.IsSelected = FirstTab.IsVisible;
        }
        
        private void AddTab_OnClick(object sender, RoutedEventArgs e)
        {
//            var newTab = new RibbonTabViewModel
//            {
//                Name = "NewTab" + Tabs.Count,
//                Text = "NewTab" + Tabs.Count,
//                IsVisible = true,
//                Bars = {_mainBar}
//            };
//            
//            Tabs.Add(newTab);
        }
    }
}