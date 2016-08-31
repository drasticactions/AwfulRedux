using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using AwfulForumsLibrary.Managers;
using AwfulForumsLibrary.Models.Web;
using AwfulRedux.Database;
using AwfulRedux.Services.WindowService;
using AwfulRedux.Tools.Authentication;
using AwfulRedux.Tools.Database;
using AwfulRedux.Tools.Errors;
using AwfulRedux.Tools.Web;
using AwfulRedux.UI;
using AwfulRedux.UI.Models.Posts;
using AwfulRedux.UI.Models.Threads;
using AwfulRedux.Views;
using AwfulWebTemplate;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using RefreshableListView;
using SQLite.Net.Platform.WinRT;
using Template10.Mvvm;

namespace AwfulRedux.ViewModels
{
    public class ThreadViewModel : ViewModelBase
    {
        private readonly AuthenticatedUserDatabase _udb = new AuthenticatedUserDatabase(new SQLitePlatformWinRT(), DatabaseWinRTHelpers.GetWinRTDatabasePath("ForumsRedux.db"));

        private Thread _selected = default(Thread);

        public Thread Selected
        {
            get { return _selected; }
            set
            {
                Set(ref _selected, value);
            }
        }

        private string _pageSelection;

        public string PageSelection
        {
            get { return _pageSelection; }
            set
            {
                Set(ref _pageSelection, value);
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

        private bool _isLoading = default(bool);

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                Set(ref _isLoading, value);
            }
        }

        private PlatformIdentifier GetTheme
        {
            get
            {
                if (App.Settings.AppTheme == ApplicationTheme.Light)
                {
                    return PlatformIdentifier.Windows8;
                }

                return PlatformIdentifier.WindowsPhone;
            }
        }

        private WebManager _webManager;

        private PostManager _postManager;

        public async Task NextPage()
        {
            if (Selected.CurrentPage >= Selected.TotalPages) return;
            Selected.CurrentPage++;
            Selected.ScrollToPost = 0;
            Selected.ScrollToPostString = string.Empty;
            await LoadThread();
        }

        public async Task PreviousPage()
        {
            if (Selected.CurrentPage <= 0) return;
            Selected.CurrentPage--;
            Selected.ScrollToPost = 0;
            Selected.ScrollToPostString = string.Empty;
            await LoadThread();
        }

        public async Task LaunchAsExternalView()
        {
            var newThread = Selected.Clone();
            newThread.Html = null;
            newThread.Posts = null;
            WindowHelper helper = new WindowHelper();
            await helper.ShowAsync<ThreadPage>(JsonConvert.SerializeObject(newThread));
        }

        public async void PullToRefresh(object sender, RefreshRequestedEventArgs e)
        {
            var deferral = e.GetDeferral();
            await LoadThread();
            deferral.Complete();
        }

        public async Task LoginUser()
        {
            var cookie = await LoginHelper.LoginDefaultUser();
            if (cookie == null)
            {
                _webManager = new WebManager();
                _postManager = new PostManager(_webManager);
                return;
            }
            _webManager = new WebManager(cookie);
            _postManager = new PostManager(_webManager);
            IsLoggedIn = true;
        }

        public async Task ReloadThread()
        {
            await LoadThread();
        }

        public async Task LoadThread(bool goToPageOverride = false)
        {
            IsLoading = true;
            if (_postManager == null)
            {
                await LoginUser();
            }
            var result = await _postManager.GetThreadPostsAsync(Selected.Location, Selected.CurrentPage, Selected.HasBeenViewed, goToPageOverride);
            var resultCheck = await ResultChecker.CheckPaywallOrSuccess(result);
            if (!resultCheck)
            {
                if (result.Type == typeof(Error).ToString())
                {
                    var error = JsonConvert.DeserializeObject<Error>(result.ResultJson);
                    if (error.IsPaywall)
                    {
                        Template10.Common.BootStrapper.Current.NavigationService.Navigate(typeof(PaywallPage));
                        IsLoading = false;
                        return;
                    }
                }
            }
            var postresult = JsonConvert.DeserializeObject<ThreadPosts>(result.ResultJson);
            Selected.LoggedInUserName = postresult.ForumThread.LoggedInUserName;
            Selected.CurrentPage = postresult.ForumThread.CurrentPage;
            Selected.TotalPages = postresult.ForumThread.TotalPages;
            Selected.Posts = postresult.Posts;
            // If the user is the "Test" user, say they are not logged in (even though they are)
            if (Selected.LoggedInUserName == "Testy Susan")
            {
                IsLoggedIn = false;
            }
            var threadTemplateModel = new ThreadTemplateModel()
            {
                ForumThread = Selected,
                IsDarkThemeSet = this.GetTheme == PlatformIdentifier.WindowsPhone,
                IsLoggedIn = IsLoggedIn,
                Posts = postresult.Posts,
                EmbeddedGifv = App.Settings.ShowEmbeddedGifv,
                EmbeddedTweets = App.Settings.ShowEmbeddedTweets,
                EmbeddedVideo = App.Settings.ShowEmbeddedVideo
            };
            var threadTemplate = new ThreadTemplate() { Model = threadTemplateModel };
            Selected.Html = threadTemplate.GenerateString();
            var count = postresult.Posts.Count(node => !node.HasSeen);
            if (Selected.RepliesSinceLastOpened > 0)
            {
                if ((Selected.RepliesSinceLastOpened - count < 0) || count == 0)
                {
                    Selected.RepliesSinceLastOpened = 0;
                }
                else
                {
                    Selected.RepliesSinceLastOpened -= count;
                }
            }
            Selected.Name = postresult.ForumThread.Name;
            if (Selected.IsBookmark)
            {
                await _db.RefreshBookmark(Selected);
            }
            IsLoading = false;
            RaisePropertyChanged("Selected");
        }

        public async Task FirstThreadPage()
        {
            Selected.CurrentPage = 1;
            Selected.ScrollToPost = 0;
            Selected.ScrollToPostString = string.Empty;
            // Force the new page number.
            await LoadThread(true);
        }

        public async Task LastThreadPage()
        {
            Selected.CurrentPage = Selected.TotalPages;
            Selected.ScrollToPost = 0;
            Selected.ScrollToPostString = string.Empty;
            // Force the new page number.
            await LoadThread(true);
        }

        public async Task ChangeThreadPage()
        {
            int userInputPageNumber;
            try
            {
                userInputPageNumber = Convert.ToInt32(PageSelection);
            }
            catch (Exception)
            {
                // User entered invalid number, return.
                return;
            }

            if (userInputPageNumber < 1 || userInputPageNumber > Selected.TotalPages) return;
            Selected.CurrentPage = userInputPageNumber;
            Selected.ScrollToPost = 0;
            Selected.ScrollToPostString = string.Empty;
            // Force the new page number.
            await LoadThread(true);
        }

        public void ReplyToThread()
        {
            Template10.Common.BootStrapper.Current.NavigationService.Navigate(typeof (ReplyPage),
                JsonConvert.SerializeObject(new ThreadReply()
                {
                    Thread  = new Thread()
                    {
                        ForumId = Selected.ForumId,
                        ThreadId = Selected.ThreadId,
                        Name = Selected.Name,
                        CurrentPage = Selected.CurrentPage,
                        TotalPages = Selected.TotalPages
                    }
                }));
        }

        private readonly BookmarkDatabase _db = new BookmarkDatabase(new SQLitePlatformWinRT(), DatabaseWinRTHelpers.GetWinRTDatabasePath("BookmarkRedux.db"));

        public async void AddRemoveNotificationTable()
        {
            if (!Selected.IsBookmark)
            {
                //await
                //    AwfulDebugger.SendMessageDialogAsync(
                //        "In order to be notified of thread updates, the thread must be a bookmark.",
                //        new Exception("Not a bookmark"));
                return;
            }
            try
            {
                await _db.SetThreadNotified(Selected);
            }
            catch (Exception ex)
            {
                //await
                //   AwfulDebugger.SendMessageDialogAsync(
                //       "Failed to save thread to notifications table",
                //       ex);
            }

            var msgDlg2 =
       new MessageDialog(Selected.IsNotified ? $"You will now be notified of updates to '{Selected.Name}'." : $"'{Selected.Name}' is now removed for your notification list.")
       {
           DefaultCommandIndex = 1
       };
            await msgDlg2.ShowAsync();
        }

        public async void AddRemoveBookmark()
        {
            try
            {
                var threadManager = new ThreadManager(_webManager);
                string bookmarkstring;
                if (Selected.IsBookmark)
                {
                    await threadManager.RemoveBookmarkAsync(Selected.ThreadId);
                    Selected.IsBookmark = !Selected.IsBookmark;
                    await _db.RefreshBookmark(Selected);
                    bookmarkstring = string.Format("'{0}' has been removed from your bookmarks.", Selected.Name);
                }
                else
                {
                    bookmarkstring = string.Format("'{0}' has been added to your bookmarks.",
                        Selected.Name);
                    Selected.IsBookmark = !Selected.IsBookmark;
                    await threadManager.AddBookmarkAsync(Selected.ThreadId);
                    await _db.AddBookmark(Selected);
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
