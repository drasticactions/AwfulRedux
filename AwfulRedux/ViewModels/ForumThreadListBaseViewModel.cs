using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using AwfulRedux.Controls;
using AwfulForumsLibrary.Managers;
using AwfulRedux.Database;
using AwfulRedux.Tools.Authentication;
using AwfulRedux.Tools.Database;
using AwfulRedux.UI.Models.Threads;
using Kimono.Controls;

using Template10.Mvvm;

namespace AwfulRedux.ViewModels
{
    public class ForumThreadListBaseViewModel : ViewModelBase
    {
        public ThreadView ThreadView { get; set; }

        public MasterDetailViewControl MasterDetailViewControl { get; set; }

        public readonly BookmarkDatabase Db = new BookmarkDatabase(DatabaseWinRTHelpers.GetWinRTDatabasePath("BookmarkRedux.db"));

        private Thread _selected = default(Thread);

        public Thread Selected
        {
            get { return _selected; }
            set
            {
                Set(ref _selected, value);
            }
        }

        private bool _isThreadSelectedAndLoaded;

        public bool IsThreadSelectedAndLoaded
        {
            get { return _isThreadSelectedAndLoaded; }
            set
            {
                Set(ref _isThreadSelectedAndLoaded, value);
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

        private bool _isLoggedIn = default(bool);

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set
            {
                Set(ref _isLoggedIn, value);
            }
        }

        public async void GoToLastPage(Thread thread)
        {
            thread.CurrentPage = thread.TotalPages;
            thread.RepliesSinceLastOpened = 0;
            Selected = thread;
        }

        public WebManager WebManager { get; set; }

        public async Task LoginUser()
        {
            var cookie = await LoginHelper.LoginDefaultUser();
            if (cookie == null)
            {
                WebManager = new WebManager();
                return;
            }
            WebManager = new WebManager(cookie);
            IsLoggedIn = true;
        }

        public async void UnreadThread(Thread thread)
        {
            var threadManager = new ThreadManager(WebManager);
            await threadManager.MarkThreadUnreadAsync(thread.ThreadId);
            thread.HasBeenViewed = false;
            thread.HasSeen = false;
            thread.ReplyCount = 0;
            var msgDlg2 =
                       new MessageDialog(string.Format("'{0}' is now unread.", thread.Name))
                       {
                           DefaultCommandIndex = 1
                       };
            await msgDlg2.ShowAsync();
        }

        public async void AddRemoveBookmark(Thread thread)
        {
            try
            {
                var threadManager = new ThreadManager(WebManager);
                string bookmarkstring;
                if (thread.IsBookmark)
                {
                    await threadManager.RemoveBookmarkAsync(thread.ThreadId);
                    thread.IsBookmark = !thread.IsBookmark;
                    await Db.RefreshBookmark(thread);
                    bookmarkstring = string.Format("'{0}' has been removed from your bookmarks.", thread.Name);
                }
                else
                {
                    bookmarkstring = string.Format("'{0}' has been added to your bookmarks.",
                        thread.Name);
                    thread.IsBookmark = !thread.IsBookmark;
                    await threadManager.AddBookmarkAsync(thread.ThreadId);
                    await Db.AddBookmark(thread);
                }
                var msgDlg2 =
                       new MessageDialog(bookmarkstring)
                       {
                           DefaultCommandIndex = 1
                       };
                await msgDlg2.ShowAsync();
            }
            catch (Exception)
            {
                // TODO: Add error message
            }
        }
    }
}
