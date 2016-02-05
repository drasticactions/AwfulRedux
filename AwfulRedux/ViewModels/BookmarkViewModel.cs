using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using AmazingPullToRefresh.Controls;
using AwfulRedux.Controls;
using AwfulRedux.Core.Managers;
using AwfulRedux.Database;
using AwfulRedux.Tools.Database;
using AwfulRedux.UI.Models.Threads;
using Kimono.Controls;
using Newtonsoft.Json;
using SQLite.Net.Platform.WinRT;
using Template10.Mvvm;
using Template10.Utils;

namespace AwfulRedux.ViewModels
{
    public class BookmarkViewModel : ViewModelBase
    {
        public ThreadView ThreadView { get; set; }
        public MasterDetailViewControl MasterDetailViewControl { get; set; }
        private ObservableCollection<Thread> _bookmarkedThreads = new ObservableCollection<Thread>();

        private Thread _selected = default(Thread);

        public Thread Selected
        {
            get { return _selected; }
            set
            {
                Set(ref _selected, value);
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

        public ObservableCollection<Thread> BookmarkedThreads
        {
            get { return _bookmarkedThreads; }
            set
            {
                Set(ref _bookmarkedThreads, value);
            }
        }

        private readonly BookmarkDatabase _db = new BookmarkDatabase(new SQLitePlatformWinRT(), DatabaseWinRTHelpers.GetWinRTDatabasePath("Bookmark.db"));
        private readonly ThreadManager _threadManager = new ThreadManager(Views.Shell.Instance.ViewModel.WebManager);

        public async void PullToRefresh_ListView(object sender, RefreshRequestedEventArgs e)
        {
            var deferral = e.GetDeferral();
            await Refresh(true);
            deferral.Complete();
        }

        public override async void OnNavigatedTo(object parameter, NavigationMode mode,
            IDictionary<string, object> state)
        {
            Template10.Common.BootStrapper.Current.NavigationService.FrameFacade.BackRequested += MasterDetailViewControl.NavigationManager_BackRequested;
            if (state.ContainsKey(nameof(Selected)))
            {
                if (Selected == null)
                {
                    Selected = JsonConvert.DeserializeObject<Thread>(state[nameof(Selected)]?.ToString());
                    await ThreadView.LoadThread(Selected);
                    state.Clear();
                }
            }
            if (BookmarkedThreads != null && BookmarkedThreads.Any())
            {
                return;
            }
            IsLoading = true;
            try
            {
                var bookmarks = await _db.GetBookmarkedThreadsFromDb();
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
                    var html = Selected.Html;
                    var posts = Selected.Posts;
                    Selected.Html = null;
                    Selected.Posts = null;
                    Selected.Html = html;
                    Selected.Posts = posts;
                }

            }
            return Task.CompletedTask;
        }

        public async Task Refresh(bool isPtr = false)
        {
            IsLoading = !isPtr;
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
                await _db.RefreshBookmarkedThreads(BookmarkedThreads.ToList());
            }
            catch (Exception ex)
            {
                //AwfulDebugger.SendMessageDialogAsync("Failed to get Bookmarks", ex);
            }
            IsLoading = false;
        }
    }
}
