using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Navigation;
using AwfulRedux.Core.Managers;
using AwfulRedux.Core.Tools;
using AwfulRedux.Database;
using AwfulRedux.Tools.Database;
using AwfulRedux.Tools.Errors;
using AwfulRedux.UI.Models.Forums;
using Newtonsoft.Json;
using SQLite.Net.Platform.WinRT;
using Template10.Mvvm;
using Template10.Services.NavigationService;

namespace AwfulRedux.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public ObservableCollection<Category> ForumGroupList { get; set; }

        private Category _favoritesEntity;
        private readonly MainForumsDatabase _db = new MainForumsDatabase(new SQLitePlatformWinRT(), DatabaseWinRTHelpers.GetWinRTDatabasePath("Forums.db"));

        public override async void OnNavigatedTo(object parameter, NavigationMode mode,
            IDictionary<string, object> state)
        {
            ForumGroupList = new ObservableCollection<Category>();
            await GetFavoriteForums();
            await GetMainPageForumsAsync();
        }

        private async Task GetMainPageForumsAsync()
        {
            var forumCategoryEntities = await _db.GetMainForumsList();
            if (forumCategoryEntities.Any())
            {
                foreach (var forumCategoryEntity in forumCategoryEntities)
                {
                    ForumGroupList.Add(forumCategoryEntity);
                }
                return;
            }

            if (!Views.Shell.Instance.IsLoggedIn)
            {
                var sampleFile = @"Assets\Forums\forum.txt";
                var installationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                var file = await installationFolder.GetFileAsync(sampleFile);
                var sampleDataText = await FileIO.ReadTextAsync(file);
                forumCategoryEntities = JsonConvert.DeserializeObject<List<Category>>(sampleDataText);
            }


            foreach (var forumCategoryEntity in forumCategoryEntities)
            {
                ForumGroupList.Add(forumCategoryEntity);
            }
            RaisePropertyChanged("ForumGroupList");
            await _db.SaveMainForumsList(ForumGroupList.ToList());
        }

        private async Task GetFavoriteForums()
        {
            var forumEntities = await _db.GetFavoriteForumsAsync();
            var favorites = ForumGroupList.FirstOrDefault(node => node.Name.Equals("Favorites"));
            if (!forumEntities.Any())
            {
                if (favorites != null)
                {
                    ForumGroupList.Remove(favorites);
                }
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
        }

        public void NavigateToThreadList(Forum forum)
        {
            NavigationService.Navigate(typeof (Views.ThreadListPage), JsonConvert.SerializeObject(forum));
        }
    }
}
