using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using AwfulForumsLibrary.Managers;
using AwfulRedux.Database;
using AwfulRedux.Notifications;
using AwfulRedux.UI.Models.Threads;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
namespace AwfulRedux.BackgroundNotify
{
    public sealed class BackgroundNotifyStatus : IBackgroundTask
    {
        private ThreadManager _threadManager;
        private WebManager _webManager;
        readonly Template10.Services.SettingsService.ISettingsHelper _helper;
        private readonly MainForumsDatabase _db = new MainForumsDatabase(Path.Combine(ApplicationData.Current.LocalFolder.Path, "ForumsRedux.db"));
        private readonly AuthenticatedUserDatabase _udb = new AuthenticatedUserDatabase(Path.Combine(ApplicationData.Current.LocalFolder.Path, "ForumsRedux.db"));
        private readonly BookmarkDatabase _bdb = new BookmarkDatabase(Path.Combine(ApplicationData.Current.LocalFolder.Path, "BookmarkRedux.db"));
        public BackgroundNotifyStatus()
        {
            _helper = new Template10.Services.SettingsService.SettingsHelper();
        }

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            try
            {
                if (_helper.Read<bool>("BackgroundEnable", false))
                {
                    if (NotifyStatusTile.IsInternet())
                    {
                        await LoginUser();
                        await Update(taskInstance);
                    }
                }

            }
            catch (Exception)
            {
            }
            deferral.Complete();
        }

        private async Task LoginUser()
        {
            var defaultUsers = await _udb.GetAuthUsers();
            if (!defaultUsers.Any()) return;
            var defaultUser = defaultUsers.First();
            var cookie = await CookieManager.LoadCookie(defaultUser.Id + ".txt");
            _webManager = new WebManager(cookie);
            _threadManager = new ThreadManager(_webManager);
        }

        private void CreateBookmarkLiveTiles(IEnumerable<Thread> forumThreads)
        {
            foreach (var thread in forumThreads.Where(thread => thread.RepliesSinceLastOpened > 0))
            {
                NotifyStatusTile.CreateBookmarkLiveTile(thread);
            }
        }

        private async Task Update(IBackgroundTaskInstance taskInstance)
        {
            var newbookmarkthreads = new List<Thread>();
            try
            {
                var pageNumber = 1;
                var hasItems = false;
                while (!hasItems)
                {
                    var bookmarkResult = await _threadManager.GetBookmarksAsync(pageNumber);
                    var bookmarks = JsonConvert.DeserializeObject<List<Thread>>(bookmarkResult.ResultJson);
                    if (!bookmarks.Any())
                    {
                        hasItems = true;
                    }
                    else
                    {
                        pageNumber++;
                    }
                    newbookmarkthreads.AddRange(bookmarks);
                }
                _helper.Write<DateTime>("LastRefresh", DateTime.UtcNow);
                await _bdb.RefreshBookmarkedThreads(newbookmarkthreads);
                newbookmarkthreads = await _bdb.GetBookmarkedThreadsFromDb();
            }
            catch (Exception ex)
            {
                //AwfulDebugger.SendMessageDialogAsync("Failed to get Bookmarks", ex);
            }

            if (!newbookmarkthreads.Any())
            {
                return;
            }

            if (_helper.Read<bool>("BookmarkBackground", false))
            {
                CreateBookmarkLiveTiles(newbookmarkthreads);
            }

            if (_helper.Read<bool>("BookmarkNotifications", false))
            {
                var notifyList = newbookmarkthreads.Where(node => node.IsNotified);
                CreateToastNotifications(notifyList);
            }
        }

        private void CreateToastNotifications(IEnumerable<Thread> forumThreads)
        {
            foreach (var thread in forumThreads.Where(thread => thread.RepliesSinceLastOpened > 0))
            {
                NotifyStatusTile.CreateToastNotification(thread);
            }
        }
    }
}
