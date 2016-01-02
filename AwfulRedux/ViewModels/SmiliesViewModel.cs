using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using AwfulRedux.Core.Managers;
using AwfulRedux.UI.Models.Smilies;
using Newtonsoft.Json;
using Template10.Mvvm;
using Template10.Utils;

namespace AwfulRedux.ViewModels
{
    public class SmiliesViewModel : ViewModelBase
    {
        private ObservableCollection<SmileCategory> _smileCategoryList = new ObservableCollection<SmileCategory>();

        public ObservableCollection<SmileCategory> SmileCategoryList
        {
            get { return _smileCategoryList; }
            set
            {
                Set(ref _smileCategoryList, value);
            }
        }

        private string _postBody = default(string);

        public string PostBody
        {
            get { return _postBody; }
            set
            {
                Set(ref _postBody, value);
            }
        }

        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                Set(ref _isLoading, value);
            }
        }

        private readonly SmileManager _smileManager = new SmileManager(Views.Shell.Instance.ViewModel.WebManager);

        public ObservableCollection<SmileCategory> FullSmileCategoryEntities { get; set; }

        public async Task LoadSmilies()
        {
            if (!SmileCategoryList.Any())
            {
                IsLoading = true;
                var result = await _smileManager.GetSmileList();
                var list = JsonConvert.DeserializeObject<List<SmileCategory>>(result.ResultJson);
                FullSmileCategoryEntities = list.ToObservableCollection();
                foreach (var item in list)
                {
                    SmileCategoryList.Add(item);
                }
                IsLoading = false;
            }
        }

        public void SmiliesFilterOnSuggestedQuery(SearchBox sender, SearchBoxSuggestionsRequestedEventArgs args)
        {
            
        }

        public void SmiliesFilterOnSubmittedQuery(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            
        }

        public void SmiliesFilterOnChangedQuery(SearchBox sender, SearchBoxQueryChangedEventArgs args)
        {
            
        }
    }
}
