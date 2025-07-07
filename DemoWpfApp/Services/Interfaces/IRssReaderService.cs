using DemoWpfApp.Models;
using SimpleFeedReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWpfApp.Services.Interfaces
{
    public interface IRssReaderService
    {
        Task<IEnumerable<ExtendedFeedItem>> RetrieveAsync(string uri);
    }
}
