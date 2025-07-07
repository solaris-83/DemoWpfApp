using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoWpfApp.Models;
using DemoWpfApp.Services.Interfaces;
using SimpleFeedReader;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoWpfApp.ViewModels
{
    public partial class RssReaderViewModel : ObservableObject
    {
        private readonly IRssReaderService _feedReaderService;
        public RssReaderViewModel(IRssReaderService feedReaderService)
        {
           _feedReaderService = feedReaderService;
           ReadRssCommand.Execute(null);
        }

        [ObservableProperty]
        private string _text = "No items!";

        [ObservableProperty]
        private IEnumerable<ExtendedFeedItem> _feedItems;

        [RelayCommand]
        public async Task ReadRss()
        {
            FeedItems = await _feedReaderService.RetrieveAsync("https://scioperi.mit.gov.it/mit2/public/scioperi/rss");
        }
    }
}
