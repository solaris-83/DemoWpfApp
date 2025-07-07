using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.DependencyInjection;
using DemoWpfApp.Services.Interfaces;
using DemoWpfApp.Services;
using MVVMNavigationModule;
using DemoWpfApp.ViewModels;
using DemoWpfApp.Views;
using MVVMNavigationModule.Abstractions;
using MVVMNavigationModule.Core;
using System.Runtime.Versioning;
using SimpleFeedReader;

[assembly: SupportedOSPlatform("windows")]
namespace DemoWpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainWindow = new MainWindow();
            Ioc.Default.ConfigureServices(
                new ServiceCollection()
               .AddSingleton<INavigationManager>(_ => new NavigationManager(mainWindow.contentControl))
               .AddSingleton<MainViewModel>()
               .AddSingleton<ICustomerRepository, CustomerRepository>()
               .AddSingleton<CustomersViewModel>()
               .AddTransient<RssReaderViewModel>()
               .AddSingleton<HtmlViewModel>()
               .AddSingleton<IRssReaderService, RssReaderService>()
               .AddFeedReader(() => new FeedReaderOptions() { ThrowOnError = true })
               .BuildServiceProvider());

            var navManager = Ioc.Default.GetService<INavigationManager>();
            var vm = () => new CustomersViewModel(Ioc.Default.GetService<ICustomerRepository>());
            ((NavigationManagerBase)navManager).Register<CustomersView>(NavigationKeys.Main, vm);
            ((NavigationManagerBase)navManager).Register<RssReaderPage>(NavigationKeys.AnotherPage, Ioc.Default.GetService<RssReaderViewModel>);
            ((NavigationManagerBase)navManager).Register<HtmlPage>(NavigationKeys.HtmlPage, Ioc.Default.GetService<HtmlViewModel>);
            mainWindow.Show();
            navManager.Navigate(NavigationKeys.Main);
        }
    }

    public static class NavigationKeys
    {
        public const string Main = nameof(Main);
        public const string AnotherPage = nameof(AnotherPage);
        public const string SendMessage = nameof(SendMessage);
        public const string HtmlPage = nameof(HtmlPage);
    }
}