using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Navigation;

using AwfulForumsLibrary.Managers;
using AwfulForumsLibrary.Tools;
using AwfulRedux.Database;
using AwfulRedux.Tools.Authentication;
using AwfulRedux.Tools.Database;
using AwfulRedux.Tools.Errors;
using AwfulRedux.UI.Models.Forums;
using Newtonsoft.Json;
using RefreshableListView;
using SQLite.Net.Platform.WinRT;
using Template10.Mvvm;
using Template10.Services.NavigationService;

namespace AwfulRedux.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public ObservableCollection<Category> ForumGroupList { get; set; } = new ObservableCollection<Category>();

        private Category _favoritesEntity;
        private readonly MainForumsDatabase _db = new MainForumsDatabase(new SQLitePlatformWinRT(), DatabaseWinRTHelpers.GetWinRTDatabasePath("ForumsRedux.db"));
        private readonly AuthenticatedUserDatabase _udb = new AuthenticatedUserDatabase(new SQLitePlatformWinRT(), DatabaseWinRTHelpers.GetWinRTDatabasePath("ForumsRedux.db"));

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            if (!ForumGroupList.Any())
            {
                await GetFavoriteForums();
                await GetMainPageForumsAsync();
            }
        }

        private bool _isLoading = default(bool);

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                Set(ref _isLoading, value);
            }
        }

        public async Task RefreshForums()
        {
            await GetMainPageForumsAsync(true);
        }

        public async Task NavigateToAccount()
        {
            NavigationService.Navigate(typeof(Views.LoginPage));
        }

        public string LoadingUrl { get; set; } = "ms-appx:///Assets/Throbbers/throbber_1.gif";

        public async void PullToRefresh(object sender, RefreshRequestedEventArgs e)
        {
            var deferral = e.GetDeferral();
            await RefreshForums();
            deferral.Complete();
        }

        private async Task GetMainPageForumsAsync(bool forceRefresh = false)
        {
            IsLoading = true;
            var forumCategoryEntities = await _db.GetMainForumsList();
            if (forumCategoryEntities.Any() && !forceRefresh)
            {
                foreach (var forumCategoryEntity in forumCategoryEntities)
                {
                    ForumGroupList.Add(forumCategoryEntity);
                }
                IsLoading = false;
                return;
            }

            if (!Views.Shell.Instance.ViewModel.IsLoggedIn)
            {
                var sampleFile = @"Assets\Forums\forum.txt";
                var installationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                var file = await installationFolder.GetFileAsync(sampleFile);
                var sampleDataText = await FileIO.ReadTextAsync(file);
                forumCategoryEntities = JsonConvert.DeserializeObject<List<Category>>(sampleDataText);
            }

            if (Views.Shell.Instance.ViewModel.IsLoggedIn && forceRefresh)
            {
                var forumManager = new ForumManager(Views.Shell.Instance.ViewModel.WebManager);
                var forumResult = await forumManager.GetForumCategoriesAsync();
                var resultCheck = await ResultChecker.CheckSuccess(forumResult);
                if (!resultCheck)
                {
                    await ResultChecker.SendMessageDialogAsync("Failed to update initial forum list", false);
                    IsLoading = false;
                    return;
                }
                forumCategoryEntities = JsonConvert.DeserializeObject<List<Category>>(forumResult.ResultJson);
            }

            ForumGroupList.Clear();
            foreach (var forumCategoryEntity in forumCategoryEntities)
            {
                ForumGroupList.Add(forumCategoryEntity);
            }
            RaisePropertyChanged("ForumGroupList");
            await _db.SaveMainForumsList(ForumGroupList.ToList());
            IsLoading = false;
        }

        private async Task GetFavoriteForums()
        {
            IsLoading = true;
            var forumEntities = await _db.GetFavoriteForumsAsync();
            var favorites = ForumGroupList.FirstOrDefault(node => node.Name.Equals("Favorites"));
            if (!forumEntities.Any())
            {
                if (favorites != null)
                {
                    ForumGroupList.Remove(favorites);
                }
                IsLoading = false;
                return;
            }

            _favoritesEntity = new Category
            {
                Name = "Favorites",
                Location = string.Format(EndPoints.ForumPage, "forumid=48"),
                ForumList = forumEntities
            };

            if (favorites == null)
            {
                ForumGroupList.Insert(0, _favoritesEntity);
            }
            else
            {
                ForumGroupList.RemoveAt(0);
                ForumGroupList.Insert(0, _favoritesEntity);
            }
            IsLoading = false;
        }

        public void NavigateToThreadList(Forum forum)
        {
            NavigationService.Navigate(typeof (Views.ThreadListPage), JsonConvert.SerializeObject(forum));
        }
    }
}
