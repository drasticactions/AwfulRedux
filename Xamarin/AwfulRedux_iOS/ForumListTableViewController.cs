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
			SetTableViewProperties ();
			await _forumListTableViewSource.LoadMore();
			TableView.ReloadData();
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender) 
		{
			// Determine segue action by segue identifier.
			// Note that these segues are defined in Main.storyboard.
			switch (segue.Identifier) 
			{
				case "ForumThreadSegue":
				// the selected index path
				var indexPath = TableView.IndexPathForSelectedRow;
				// the index of the item in the collection that corresponds to the selected cell
				var itemIndex = indexPath.Row;
				// get the destination viewcontroller from the segue
				var forumthreadViewController = segue.DestinationViewController as ThreadWebViewController;
				// if the detaination viewcontrolller is not null
				if (forumthreadViewController != null && TableView.Source != null) {
					// set the acquaintance on the view controller
					forumthreadViewController.SetThread (((ForumListTableViewSource)TableView.Source).Threads[indexPath.Row], this);
				}
				break;
			}
		}

		void SetTableViewProperties()
		{
			TableView.Source = _forumListTableViewSource;

			TableView.AllowsSelection = true;

			TableView.RowHeight = 60;
		}
	}
}