using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AwfulRedux.Core.Managers;
using AwfulRedux.Database;
using AwfulRedux.Mobile.Models.Thread;
using AwfulRedux.Mobile.Services;
using AwfulRedux.Mobile.Tools;
using AwfulRedux.UI.Models.Forums;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace AwfulRedux.Mobile.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigationAware
    {
        public MainPageViewModel()
        {
            Initialize();
        }
        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            //await Initialize();
        }

        #region Database
        private readonly MainForumsDatabase _db = new MainForumsDatabase(DependencyService.Get<ISQLite>().GetPlatform(), DependencyService.Get<ISQLite>().GetPath("ForumsRedux.db"));
        private OfflineDataStore _offlineDataStore = new OfflineDataStore();
        private ObservableCollection<ForumThreadCategory> _forumCategories = new ObservableCollection<ForumThreadCategory>();
        #endregion

        public ObservableCollection<ForumThreadCategory> ForumCategories
        {
            get { return _forumCategories; }
            set
            {
                SetProperty(ref _forumCategories, value);
                OnPropertyChanged();
            }
        }

        public Action<Forum> ItemSelected { get; set; }

        private Forum _selectedForum;

        public Forum SelectedForum
        {
            get { return _selectedForum; }
            set
            {
                _selectedForum = value;

                if (SelectedForum == null)
                    return;
                if (ItemSelected == null)
                {
                    //page.Navigation.PushAsync(new ForumListPage(_selectedForum));
                    _selectedForum = null;
                }
                else
                {
                    ItemSelected.Invoke(_selectedForum);
                }
                OnPropertyChanged();
            }
        }

        public async Task Initialize(bool forceRefresh = false)
        {
            ForumCategories = new ObservableCollection<ForumThreadCategory>();
            var forumCategoryEntities = await _db.GetMainForumsList();
            if (forumCategoryEntities.Any() && !forceRefresh)
            {
                foreach (var forum in forumCategoryEntities)
                {
                    var cat = new ForumThreadCategory()
                    {
                        Location = forum.Location,
                        Name = forum.Name,
                        Order = forum.Order
                    };
                    foreach (var item in forum.ForumList)
                    {

                        cat.Add(item);
                    }
                    ForumCategories.Add(cat);
                }
                return;
            }

            if (!App.IsLoggedIn)
            {
                var forums = await _offlineDataStore.GetDefaultForumList();
                foreach (var forum in forums)
                {
                    var cat = new ForumThreadCategory()
                    {
                        Location = forum.Location,
                        Name = forum.Name,
                        Order = forum.Order
                    };
                    foreach (var item in forum.ForumList)
                    {
                        cat.Add(item);
                    }
                    ForumCategories.Add(cat);
                }
            }
            else if (App.IsLoggedIn)
            {
                var forumManager = new ForumManager(App.WebManager);
                var forumResult = await forumManager.GetForumCategoriesAsync();
                var newEntities = JsonConvert.DeserializeObject<List<AwfulRedux.UI.Models.Forums.Category>>(forumResult.ResultJson);
                foreach (var forum in newEntities)
                {
                    var cat = new ForumThreadCategory()
                    {
                        Location = forum.Location,
                        Name = forum.Name,
                        Order = forum.Order
                    };
                    foreach (var item in forum.ForumList)
                    {
                        cat.Add(item);
                    }
                    ForumCategories.Add(cat);
                }
                await _db.SaveMainForumsList(newEntities);
            }
        }
    }
}
