using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AwfulForumsLibrary.Managers;
using AwfulRedux.UI.Models.Forums;
using AwfulRedux.UI.Models.Threads;
using Foundation;
using Newtonsoft.Json;
using UIKit;

namespace AwfulRedux_iOS
{
	public class ForumListTableViewSource : UITableViewSource
	{
		public ForumListTableViewSource (Forum forum)
		{
			Forum = forum;
			pageIndex = 0;
			Threads = new List<Thread>();
		}

		public Forum Forum { get; set;}

		public List<Thread> Threads { get; set; }

		int pageIndex;
		bool isFetching;

		ThreadManager _threadManager = new ThreadManager(AppDelegate.WebManager);

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			// try to get a cell that's currently off-screen, not being displayed
			var cell = tableView.DequeueReusableCell("ForumThreadCell", indexPath) as ForumThreadCell;

			// get an item from the collection, using the table row index as the collection index
			var thread = Threads[indexPath.Row];

			cell.Update(thread);

			if (!isFetching && indexPath.Row > Threads.Count * 0.8)
			{
				isFetching = true;
				Task.Factory.StartNew(LoadMore);
			}

			return cell;
		}

		public async Task LoadMore()
		{
			var result = await _threadManager.GetForumThreadsAsync(Forum.Location, Forum.ForumId, pageIndex);
			var forumThreadEntities = JsonConvert.DeserializeObject<List<Thread>>(result.ResultJson);
			Threads.AddRange(forumThreadEntities);
			pageIndex++;
			isFetching = false;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return Threads.Count;
		}
	}
}
