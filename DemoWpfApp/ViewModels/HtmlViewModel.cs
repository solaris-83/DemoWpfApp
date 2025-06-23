using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoWpfApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace DemoWpfApp.ViewModels
{
    public partial class HtmlViewModel : ObservableObject
    {
        public HtmlViewModel()
        {
           
        }
        [ObservableProperty]
        private string url = "";

        [ObservableProperty]
        private string title = "WebView2 Browser";

        [ObservableProperty]
        private bool canGoBack;

        [ObservableProperty]
        private bool canGoForward;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string addressBarText = ""; // "https://www.microsoft.com";

        [ObservableProperty]
        private string htmlContent = File.ReadAllText( @"Resources\Page1.html"); // DefaultHtmlContent;

        [ObservableProperty]
        private string receivedMessage = "";

        [ObservableProperty]
        private string messageToSend = "Hello from C#!";

        [ObservableProperty]
        private UserInfo currentUser = new UserInfo { Name = "John Doe", Email = "john@example.com", Age = 30 };

        [ObservableProperty]
        private ObservableCollection<string> messageLog = new ObservableCollection<string>();

        // Events for View to subscribe to
        public event EventHandler<string> NavigateRequested;
        public event EventHandler BackRequested;
        public event EventHandler ForwardRequested;
        public event EventHandler RefreshRequested;
        public event EventHandler<string> NavigateToHtmlRequested;
        public event EventHandler<string> ExecuteScriptRequested;
        public event EventHandler<WebMessage> MessageReceived;

        [RelayCommand]
        private void Navigate(string urlToNavigate = null)
        {
            urlToNavigate ??= AddressBarText;

            if (!string.IsNullOrWhiteSpace(urlToNavigate))
            {
                if (!urlToNavigate.StartsWith("http://") && !urlToNavigate.StartsWith("https://"))
                {
                    urlToNavigate = "https://" + urlToNavigate;
                }

                NavigateRequested?.Invoke(this, urlToNavigate);
            }
        }

        [RelayCommand(CanExecute = nameof(CanGoBack))]
        private void GoBack()
        {
            BackRequested?.Invoke(this, EventArgs.Empty);
        }

        [RelayCommand(CanExecute = nameof(CanGoForward))]
        private void GoForward()
        {
            ForwardRequested?.Invoke(this, EventArgs.Empty);
        }

        [RelayCommand]
        private void Refresh()
        {
            RefreshRequested?.Invoke(this, EventArgs.Empty);
        }

        [RelayCommand]
        private void GoHome()
        {
            NavigateRequested?.Invoke(this, "https://www.microsoft.com");
        }

        [RelayCommand]
        private void LoadCustomHtml()
        {
            NavigateToHtmlRequested?.Invoke(this, HtmlContent);
        }

        [RelayCommand]
        private void SendMessageToHtml()
        {
            if (!string.IsNullOrWhiteSpace(MessageToSend))
            {
                var message = new WebMessage
                {
                    Type = "fromCSharp",
                    Data = MessageToSend
                };

                var json = JsonSerializer.Serialize(message);
                var script = $"window.receiveMessageFromCSharp({json});";

                ExecuteScriptRequested?.Invoke(this, script);

                MessageLog.Add($"Sent to HTML: {MessageToSend}");
                MessageToSend = "";
            }
        }

        [RelayCommand]
        private void SendUserInfoToHtml()
        {
            var message = new WebMessage
            {
                Type = "userInfo",
                Data = CurrentUser
            };

            var json = JsonSerializer.Serialize(message);
            var script = $"window.receiveUserInfo({json});";

            ExecuteScriptRequested?.Invoke(this, script);
            MessageLog.Add($"Sent user info: {CurrentUser.Name}");
        }

        [RelayCommand]
        private void RequestDataFromHtml()
        {
            var script = "window.sendDataToCSharp();";
            ExecuteScriptRequested?.Invoke(this, script);
            MessageLog.Add("Requested data from HTML");
        }

        [RelayCommand]
        private void ClearMessageLog()
        {
            MessageLog.Clear();
        }

        // Methods called by the View when WebView2 events occur
        public void OnNavigationStarting(string uri)
        {
            IsLoading = true;
            AddressBarText = uri;
        }

        public void OnNavigationCompleted(string uri, string title)
        {
            IsLoading = false;
            Url = uri;
            Title = title;
            AddressBarText = uri;
        }

        public void OnHistoryChanged(bool canGoBack, bool canGoForward)
        {
            CanGoBack = canGoBack;
            CanGoForward = canGoForward;
        }

        public void OnWebMessageReceived(string message)
        {
            try
            {
                var webMessage = JsonSerializer.Deserialize<WebMessage>(message);

                switch (webMessage.Type)
                {
                    case "formData":
                        var formData = JsonSerializer.Deserialize<FormData>(webMessage.Data.ToString());
                        MessageLog.Add($"Received form data: {formData.Field1}, {formData.Field2}, Checked: {formData.Checkbox}");
                        break;

                    case "buttonClick":
                        //if (webMessage.Data != null && webMessage.Data.ToString() == "Page2.html")
                        //    this.Url = Path.GetFullPath("Resources\\Page2.html");
                        if (webMessage.Data != null )
                            this.Url = Path.GetFullPath($"Resources\\{webMessage.Data.ToString()}");
                        MessageLog.Add($"Button clicked: {webMessage.Data}");
                        break;

                    case "pageLoad":
                        MessageLog.Add("HTML page loaded and ready");
                        break;

                    default:
                        MessageLog.Add($"Received: {webMessage.Type} - {webMessage.Data}");
                        break;
                }

                ReceivedMessage = $"Last: {webMessage.Type} - {webMessage.Data}";
                MessageReceived?.Invoke(this, webMessage);
            }
            catch (Exception ex)
            {
                MessageLog.Add($"Error parsing message: {ex.Message}");
            }
        }
    }
}