using CommunityToolkit.Mvvm.ComponentModel;

namespace DemoWpfApp.ViewModels
{
    public partial class AnotherPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _text = "I am another page";
    }
}
