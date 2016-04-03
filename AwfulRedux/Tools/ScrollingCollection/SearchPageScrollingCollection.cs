using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;
using AwfulRedux.Core.Managers;
using AwfulRedux.Core.Models.Search;

namespace AwfulRedux.Tools.ScrollingCollection
{
    public class SearchPageScrollingCollection : ObservableCollection<SearchEntity>, ISupportIncrementalLoading
    {
        public SearchPageScrollingCollection(List<int> forumIds, string searchQuery)
        {
            _forumIds = forumIds;
            _searchQuery = searchQuery;
            HasMoreItems = true;
            IsLoading = false;
        }

        private List<int> _forumIds;
        private string _searchQuery;
        private int _pageCount = 2;
        private readonly SearchManager _searchManager = new SearchManager(Views.Shell.Instance.ViewModel.WebManager);
        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }

            private set
            {
                _isLoading = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsLoading"));
            }
        }

        private bool _isEmpty;
        public bool IsEmpty
        {
            get { return _isEmpty; }

            private set
            {
                _isEmpty = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsLoading"));
            }
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return LoadDataAsync(count).AsAsyncOperation();
        }

        public async Task<LoadMoreItemsResult> LoadDataAsync(uint count)
        {
            IsLoading = true;
            try
            {
                if (!this.Any())
                {
                    await InitSearchResults();
                }
                else
                {
                    await SearchViaRedirect();
                }
            }
            catch (Exception ex)
            {
               // AwfulDebugger.SendMessageDialogAsync("Failed to get search results", ex);
            }
            IsLoading = false;
            return new LoadMoreItemsResult { Count = count };
        }

        private async Task SearchViaRedirect()
        {
            if (string.IsNullOrEmpty(_redirectUrl))
            {
                return;
            }

            var results = await _searchManager.GetSearchQueryResultsViaRedirect(_redirectUrl + "&page=" + _pageCount);
            if (results != null && results.SearchEntities.Any())
            {
                HasMoreItems = true;
                foreach (var result in results.SearchEntities)
                {
                    Add(result);
                }
            }
            else
            {
                HasMoreItems = false;
            }
        }

        private string _redirectUrl;

        private async Task InitSearchResults()
        {
            var results = await _searchManager.GetSearchQueryResults(_forumIds, _searchQuery);
            if (results != null && results.SearchEntities.Any())
            {
                _redirectUrl = results.LinkUrl;
                HasMoreItems = true;
                foreach (var result in results.SearchEntities)
                {
                    Add(result);
                }
            }
            else
            {
                IsEmpty = true;
                HasMoreItems = false;
            }
        }
        public bool HasMoreItems { get; private set; }
    }
}
