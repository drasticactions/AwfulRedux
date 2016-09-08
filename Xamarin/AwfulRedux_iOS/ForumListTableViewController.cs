using Foundation;
using System;
using UIKit;
using AwfulRedux.UI.Models.Forums;

namespace AwfulRedux_iOS
{
	public partial class ForumListTableViewController : UITableViewController
	{
		public Forum Forum { get; private set; }

		public ForumListTableViewController (IntPtr handle) : base (handle)
		{
		}

		MainForumsViewController _ListViewController;

		ForumListTableViewSource _forumListTableViewSource;

		public void SetForum (Forum forum, MainForumsViewController listViewController) 
		{
			Forum = forum;

			_forumListTableViewSource = new ForumListTableViewSource (Forum);

			if (listViewController != null)
				_ListViewController = listViewController;
		}

		public override async void ViewWillAppear(bool animated)
		{
			if (Forum == null) {
				return;
			}
			TableView.Source = _forumListTableViewSource;
			await _forumListTableViewSource.LoadMore();
			TableView.ReloadData();
		}
	}
}