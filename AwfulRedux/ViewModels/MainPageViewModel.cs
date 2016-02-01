﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Navigation;
using AmazingPullToRefresh.Controls;
using AwfulRedux.Core.Managers;
using AwfulRedux.Core.Tools;
using AwfulRedux.Database;
using AwfulRedux.Tools.Authentication;
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
        public ObservableCollection<Category> ForumGroupList { get; set; } = new ObservableCollection<Category>();

        private Category _favoritesEntity;
        private readonly MainForumsDatabase _db = new MainForumsDatabase(new SQLitePlatformWinRT(), DatabaseWinRTHelpers.GetWinRTDatabasePath("Forums.db"));
        private readonly AuthenticatedUserDatabase _udb = new AuthenticatedUserDatabase(new SQLitePlatformWinRT(), DatabaseWinRTHelpers.GetWinRTDatabasePath("Forums.db"));

        public override async void OnNavigatedTo(object parameter, NavigationMode mode,
            IDictionary<string, object> state)
        {
            if (!ForumGroupList.Any())
            {
                await GetFavoriteForums();
                await GetMainPageForumsAsync();
            }
        }

        public async Task RefreshForums()
        {
            Views.Shell.ShowBusy(true, "Refreshing Forum List...");
            await GetMainPageForumsAsync(true);
            Views.Shell.ShowBusy(false);
        }

        public async void PullToRefresh(object sender, RefreshRequestedEventArgs e)
        {
            var deferral = e.GetDeferral();
            await RefreshForums();
            deferral.Complete();
        }

        private async Task GetMainPageForumsAsync(bool forceRefresh = false)
        {
            var forumCategoryEntities = await _db.GetMainForumsList();
            if (forumCategoryEntities.Any() && !forceRefresh)
            {
                foreach (var forumCategoryEntity in forumCategoryEntities)
                {
                    ForumGroupList.Add(forumCategoryEntity);
                }
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
                    Views.Shell.ShowBusy(false);
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
