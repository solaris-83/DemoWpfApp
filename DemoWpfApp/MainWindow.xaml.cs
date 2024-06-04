using CommunityToolkit.Mvvm.DependencyInjection;
using DemoWpfApp.ViewModels;

namespace DemoWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded +=
                (sender, args) => { 
                    DataContext = Ioc.Default.GetService<MainViewModel>();
                };
        }
    }
}
