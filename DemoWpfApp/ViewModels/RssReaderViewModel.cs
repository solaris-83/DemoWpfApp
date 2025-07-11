using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoWpfApp.Models;
using DemoWpfApp.Services.Interfaces;
using SimpleFeedReader;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DemoWpfApp.ViewModels
{
    public partial class RssReaderViewModel : ObservableObject
    {
        private readonly IRssReaderService _feedReaderService;
        // Combobox
        public List<string> Sectors { get; set; } = new List<string>();
        // DataGrid
        private ICollectionView _feedItemsView;

        public ICollectionView FeedItemsView => _feedItemsView;

        public RssReaderViewModel(IRssReaderService feedReaderService)
        {
            LoadSettori();
            _feedReaderService = feedReaderService;
            
            ReadRssCommand.Execute(null);
        }

        private bool FilterFeedItemPredicate(object obj)
        {
            if (obj is ExtendedFeedItem feedItem)
            {
                if (string.IsNullOrEmpty(SelectedSector))
                   return true;

                //return feedItem.Settore.Equals(feedItem.Settore, StringComparison.OrdinalIgnoreCase);
                return feedItem.Settore == SelectedSector;

            }

            return false;
        }

        [ObservableProperty]
        private string _selectedSector;

        [ObservableProperty]
        private string _text = "No items!";

        [ObservableProperty]
        private ObservableCollection<ExtendedFeedItem> _feedItems;

        [RelayCommand]
        public async Task ReadRss()
        {
            FeedItems = new ObservableCollection<ExtendedFeedItem>(await _feedReaderService.RetrieveAsync("https://scioperi.mit.gov.it/mit2/public/scioperi/rss"));
            _feedItemsView = CollectionViewSource.GetDefaultView(FeedItems);
            _feedItemsView.Filter = FilterFeedItemPredicate;
        }

        partial void OnSelectedSectorChanged(string value)
        {
            FeedItemsView.Refresh();
        }

        private void LoadSettori()
        {
            Sectors.Add("Generale");
            Sectors.Add("Plurisettoriale");
            Sectors.Add("Ferroviario");
            Sectors.Add("Appalti ferroviari");
            Sectors.Add("Trasporto pubblico locale");
            Sectors.Add("Marittimo");
            Sectors.Add("Trasporto merci");
            Sectors.Add("Elicotteri");
            Sectors.Add("Taxi");
            Sectors.Add("Ncc");
            Sectors.Add("Circolazione e sicurezza stradale");
            Sectors.Add("Aereo");
            Sectors = Sectors.OrderBy(s => s).ToList();
        }
    }
}
