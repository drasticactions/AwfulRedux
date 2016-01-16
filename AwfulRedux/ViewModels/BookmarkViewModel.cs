using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using AwfulRedux.Core.Managers;
using AwfulRedux.Database;
using AwfulRedux.Tools.Database;
using AwfulRedux.UI.Models.Threads;
using Newtonsoft.Json;
using SQLite.Net.Platform.WinRT;
using Template10.Mvvm;
using Template10.Utils;

namespace AwfulRedux.ViewModels
{
    public class BookmarkViewModel : ViewModelBase
    {
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

        public override async void OnNavigatedTo(object parameter, NavigationMode mode,
            IDictionary<string, object> state)
        {
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
            IsLoading = false;
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

                    App.Settings.LastRefresh = DateTime.UtcNow;
                }
            }
            catch (Exception ex)
            {
                //AwfulDebugger.SendMessageDialogAsync("Failed to get Bookmarks", ex);
            }
            IsLoading = false;
        }
    }
}
