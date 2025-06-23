
using DemoWpfApp.ViewModels;
using Microsoft.Web.WebView2.Core;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DemoWpfApp.Views
{
    /// <summary>
    /// Interaction logic for HtmlPage.xaml
    /// </summary>
    public partial class HtmlPage : UserControl
    {
        private HtmlViewModel _viewModel;

        public HtmlPage()
        {
            InitializeComponent();
        }

        private async void InitializeWebView()
        {
            await WebView.EnsureCoreWebView2Async();

            // Subscribe to WebView2 events
            WebView.NavigationStarting += WebView_NavigationStarting;
            WebView.NavigationCompleted += WebView_NavigationCompleted;
            WebView.CoreWebView2.HistoryChanged += WebView_HistoryChanged;

            // Enable web message communication
            WebView.CoreWebView2.WebMessageReceived += WebView_WebMessageReceived;

            _viewModel.LoadCustomHtmlCommand.Execute(default);
            // Add host objects if needed
            //WebView.CoreWebView2.AddHostObjectToScript("hostObject", _viewModel);
        }

        private void WebView_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            _viewModel.OnNavigationStarting(e.Uri);
        }

        private void OnNavigateRequested(object sender, string url)
        {
            WebView.CoreWebView2.Navigate(url);
        }

        private void OnBackRequested(object sender, EventArgs e)
        {
            if (WebView.CoreWebView2.CanGoBack)
                WebView.CoreWebView2.GoBack();
        }

        private void OnForwardRequested(object sender, EventArgs e)
        {
            if (WebView.CoreWebView2.CanGoForward)
                WebView.CoreWebView2.GoForward();
        }

        private void OnRefreshRequested(object sender, EventArgs e)
        {
            WebView.CoreWebView2.Reload();
        }

        private void OnNavigateToHtmlRequested(object sender, string html)
        {
            WebView.NavigateToString(html);
        }

        private async void OnExecuteScriptRequested(object sender, string script)
        {
            try
            {
                await WebView.CoreWebView2.ExecuteScriptAsync(script);
            }
            catch (Exception ex)
            {
                _viewModel.MessageLog.Add($"Script error: {ex.Message}");
            }
        }

        private void WebView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            var uri = WebView.CoreWebView2.Source;
            var title = WebView.CoreWebView2.DocumentTitle;
            _viewModel.OnNavigationCompleted(uri, title);
        }

        private void WebView_HistoryChanged(object sender, object e)
        {
            _viewModel.OnHistoryChanged(
                WebView.CoreWebView2.CanGoBack,
                WebView.CoreWebView2.CanGoForward);
        }

        private void WebView_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            var message = e.TryGetWebMessageAsString();
            _viewModel.OnWebMessageReceived(message);
        }

        private void AddressBar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _viewModel.NavigateCommand.Execute(_viewModel.AddressBarText);
            }
        }

        private void MessageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _viewModel.SendMessageToHtmlCommand.Execute(null);
            }
        }

        private void WebView_Unloaded(object sender, RoutedEventArgs e)
        {
            WebView.NavigationStarting -= WebView_NavigationStarting;
            WebView.NavigationCompleted -= WebView_NavigationCompleted;
            WebView.CoreWebView2.HistoryChanged -= WebView_HistoryChanged;

            // Enable web message communication
            WebView.CoreWebView2.WebMessageReceived -= WebView_WebMessageReceived;

            _viewModel.NavigateRequested -= OnNavigateRequested;
            _viewModel.BackRequested -= OnBackRequested;
            _viewModel.ForwardRequested -= OnForwardRequested;
            _viewModel.RefreshRequested -= OnRefreshRequested;
            _viewModel.NavigateToHtmlRequested -= OnNavigateToHtmlRequested;
            _viewModel.ExecuteScriptRequested -= OnExecuteScriptRequested;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel = DataContext as HtmlViewModel;

            // Subscribe to ViewModel events
            _viewModel.NavigateRequested += OnNavigateRequested;
            _viewModel.BackRequested += OnBackRequested;
            _viewModel.ForwardRequested += OnForwardRequested;
            _viewModel.RefreshRequested += OnRefreshRequested;
            _viewModel.NavigateToHtmlRequested += OnNavigateToHtmlRequested;
            _viewModel.ExecuteScriptRequested += OnExecuteScriptRequested;

            InitializeWebView();
        }
    }
}
