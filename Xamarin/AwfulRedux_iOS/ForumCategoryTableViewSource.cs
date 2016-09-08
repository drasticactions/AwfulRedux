using System;
using Foundation;
using UIKit;
using System.Collections.Generic;
using System.Threading.Tasks;
using AwfulRedux.UI.Models.Forums;

namespace AwfulRedux_iOS
{
	public class ForumCategoryTableViewSource : UITableViewSource
	{
		public List<Category> ForumCategories { get; private set; }

		public ForumCategoryTableViewSource ()
		{
			ForumCategories = new List<Category>();
		}

		public async Task LoadForumList () 
		{
			ForumCategories = await ForumDataSource.GetForumCategories(AppDelegate.WebManager);
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			// try to get a cell that's currently off-screen, not being displayed
			var cell = tableView.DequeueReusableCell("ForumCategoryCell", indexPath) as ForumCategoryCell;

			// get an item from the collection, using the table row index as the collection index
			var forum = ForumCategories[indexPath.Section].ForumList[indexPath.Row];

			cell.Update(forum);

			return cell;
		}

		public override string TitleForHeader (UITableView tableView, nint section)
		{
			return ForumCategories[(int)section].Name;
		}

		public override nint NumberOfSections (UITableView tableView)
		{
			return ForumCategories.Count;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return ForumCategories[(int)section].ForumList.Count;
		}
	}
}
