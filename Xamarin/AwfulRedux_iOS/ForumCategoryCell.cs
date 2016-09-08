using Foundation;
using System;
using UIKit;
using AwfulRedux.UI.Models.Forums;

namespace AwfulRedux_iOS
{
	public partial class ForumCategoryCell : UITableViewCell
	{
		public ForumCategoryCell (IntPtr handle) : base (handle)
		{
		}

		public void Update (Forum forum)
		{
			ForumName.Text = forum.Name;
		}
	}
}