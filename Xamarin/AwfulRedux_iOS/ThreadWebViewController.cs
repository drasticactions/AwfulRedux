using Foundation;
using System;
using UIKit;
using AwfulRedux.UI.Models.Threads;
using Newtonsoft.Json;
using AwfulRedux.Database;
using AwfulForumsLibrary.Managers;
using System.Threading.Tasks;
using AwfulForumsLibrary.Models.Web;
using AwfulRedux.UI.Models.Posts;
using AwfulWebTemplate;
using System.Linq;

namespace AwfulRedux_iOS
{
	public partial class ThreadWebViewController : UIViewController
	{
		public ThreadWebViewController (IntPtr handle) : base (handle)
		{
		}

		public bool IsLoggedIn { get; set; }

		public bool GoToPageOverride { get; set; }

		public bool IsLoading { get; set; }

		public async Task LoginUser()
		{
			_postManager = new PostManager(AppDelegate.WebManager);
			IsLoggedIn = await AuthDataSource.AreAuthUsers();
		}

		private PostManager _postManager;

		private readonly AuthenticatedUserDatabase _udb = new AuthenticatedUserDatabase(DatabaseHelpers.GetiOSDatabasePath("ForumsRedux.db"));

		private readonly BookmarkDatabase _db = new BookmarkDatabase(DatabaseHelpers.GetiOSDatabasePath("BookmarkRedux.db"));

		public Thread Selected { get; private set; }

		public void SetThread (Thread thread, ForumListTableViewController threadViewController) 
		{
			Selected = thread;
		}

		public override async void ViewWillAppear(bool animated) 
		{
			if (Selected == null) {
				return;
			}
			if (_postManager == null)
			{
				await LoginUser();
			}
			var result = await _postManager.GetThreadPostsAsync(Selected.Location, Selected.CurrentPage, Selected.HasBeenViewed, GoToPageOverride);
			var resultCheck = await ResultChecker.CheckPaywallOrSuccess(result);
			if (!resultCheck)
			{
				if (result.Type == typeof(Error).ToString())
				{
					var error = JsonConvert.DeserializeObject<Error>(result.ResultJson);
					if (error.IsPaywall)
					{
						//Template10.Common.BootStrapper.Current.NavigationService.Navigate(typeof(PaywallPage));
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
				IsDarkThemeSet = false,
				IsLoggedIn = IsLoggedIn,
				Posts = postresult.Posts,
				EmbeddedGifv = true,
				EmbeddedTweets = true,
				EmbeddedVideo = true
			};
			var threadTemplate = new ThreadTemplate() { Model = threadTemplateModel };
			Selected.Html = threadTemplate.GenerateString();
			ThreadWebView.LoadHtmlString(Selected.Html, null);
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
		}
	}
}