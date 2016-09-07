using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

using AwfulRedux.Controls;
using AwfulForumsLibrary.Managers;
using AwfulRedux.Database;
using AwfulRedux.Tools.Database;
using AwfulRedux.UI.Models.Threads;
using Kimono.Controls;
using Newtonsoft.Json;
using RefreshableListView;

using Template10.Mvvm;
using Template10.Utils;

namespace AwfulRedux.ViewModels
{
    public class BookmarkViewModel : ForumThreadListBaseViewModel
    {
        private ObservableCollection<Thread> _bookmarkedThreads = new ObservableCollection<Thread>();


        public ObservableCollection<Thread> BookmarkedThreads
        {
            get { return _bookmarkedThreads; }
            set
            {
                Set(ref _bookmarkedThreads, value);
            }
        }

        private readonly ThreadManager _threadManager = new ThreadManager(Views.Shell.Instance.ViewModel.WebManager);

        public async void PullToRefresh_ListView(object sender, EventArgs e)
        {
            await Refresh();
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            if (WebManager == null)
            {
                await LoginUser();
            }
            Template10.Common.BootStrapper.Current.NavigationService.FrameFacade.BackRequested += MasterDetailViewControl.NavigationManager_BackRequested;
            if (suspensionState.ContainsKey(nameof(Selected)))
            {
                if (Selected == null)
                {
                    Selected = JsonConvert.DeserializeObject<Thread>(suspensionState[nameof(Selected)]?.ToString());
                    await ThreadView.LoadThread(Selected, true);
                    IsThreadSelectedAndLoaded = true;
                    suspensionState.Clear();
                }
            }
            if (BookmarkedThreads != null && BookmarkedThreads.Any())
            {
                return;
            }
            IsLoading = true;
            try
            {
                var bookmarks = await Db.GetBookmarkedThreadsFromDb();
                if (bookmarks != null && bookmarks.Any())
                {
                    BookmarkedThreads = bookmarks.ToObservableCollection();
                }
                if ((!BookmarkedThreads.Any() || App.Settings.LastRefresh > (DateTime.UtcNow.AddHours(1.00))))
                {
                    await Refresh();
                }
            }
            catch (Exception ex)
            {
                //AwfulDebugger.SendMessageDialogAsync("Failed to get Bookmarks", ex);
            }

            var threadId = (long?) parameter;
            if (threadId > 0)
            {
                var thread = BookmarkedThreads.FirstOrDefault(node => node.ThreadId == threadId);
                if (thread != null)
                {
                    Selected = thread;
                    await ThreadView.LoadThread(thread);
                }
            }
            
            IsLoading = false;
        }

        public override Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            Template10.Common.BootStrapper.Current.NavigationService.FrameFacade.BackRequested -= MasterDetailViewControl.NavigationManager_BackRequested;
            if (suspending)
            {
                if (Selected != null)
                {
                    var newThread = Selected.Clone();
                    newThread.Html = null;
                    newThread.Posts = null;
                    state[nameof(Selected)] = JsonConvert.SerializeObject(newThread);
                }

            }
            return Task.CompletedTask;
        }

        public async Task Refresh()
        {
            IsLoading = true;
            try
            {
                var pageNumber = 1;
                var hasItems = false;
                var oldList = true;
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

                    if (oldList)
                    {
                        BookmarkedThreads = new ObservableCollection<Thread>();
                        oldList = false;
                    }

                    foreach (var bookmark in bookmarks)
                    {
                        BookmarkedThreads.Add(bookmark);
                    }
                }
                App.Settings.LastRefresh = DateTime.UtcNow;
                await Db.RefreshBookmarkedThreads(BookmarkedThreads.ToList());
            }
            catch (Exception ex)
            {
                //AwfulDebugger.SendMessageDialogAsync("Failed to get Bookmarks", ex);
            }
            IsLoading = false;
        }
    }
}
