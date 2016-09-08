using Foundation;
using System;
using UIKit;

namespace AwfulRedux_iOS
{
	public partial class MainForumsViewController : UITableViewController
	{
		public MainForumsViewController (IntPtr handle) : base (handle)
		{
			_forumCategoryTableViewSource = new ForumCategoryTableViewSource();
		}

		readonly ForumCategoryTableViewSource _forumCategoryTableViewSource;

		public override async void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// tell the table view source to load the data
			await _forumCategoryTableViewSource.LoadForumList ();

			SetTableViewProperties ();

			NavigationItem.BackBarButtonItem = new UIBarButtonItem ("List", UIBarButtonItemStyle.Plain, null);
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender) 
		{
			// Determine segue action by segue identifier.
			// Note that these segues are defined in Main.storyboard.
			switch (segue.Identifier) 
			{
				case "ForumListSegue":
				// the selected index path
				var indexPath = TableView.IndexPathForSelectedRow;
				// the index of the item in the collection that corresponds to the selected cell
				var itemIndex = indexPath.Row;
				// get the destination viewcontroller from the segue
				var forumListTableViewController = segue.DestinationViewController as ForumListTableViewController;
				// if the detaination viewcontrolller is not null
				if (forumListTableViewController != null && TableView.Source != null) {
					// set the acquaintance on the view controller
					forumListTableViewController.SetForum (((ForumCategoryTableViewSource)TableView.Source).ForumCategories [indexPath.Section].ForumList[indexPath.Row], this);
				}
				break;
			}
		}

		void SetTableViewProperties()
		{
			TableView.Source = _forumCategoryTableViewSource;

			TableView.AllowsSelection = true;

			TableView.RowHeight = 40;
		}
	}
}