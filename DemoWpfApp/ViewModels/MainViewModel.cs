using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MVVMNavigationModule.Abstractions;
using MVVMNavigationModule.Core;
using DemoWpfApp.Models;

namespace DemoWpfApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public MainViewModel(INavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        private INavigationManager _navigationManager;

        [RelayCommand]
        private void MainWindow()
        {
            _navigationManager.Navigate(NavigationKeys.Main);
        }

        [RelayCommand]
        private void AnotherPage()
        {
           _navigationManager.Navigate(NavigationKeys.AnotherPage);
        }

        [RelayCommand]
        private void HtmlPage()
        {
            _navigationManager.Navigate(NavigationKeys.HtmlPage);
        }

        [RelayCommand]
        private void SendMessage()
        {
            WeakReferenceMessenger.Default.Send(new LoggedInUserChangedMessage("John Doe"));
        }
    }
}
