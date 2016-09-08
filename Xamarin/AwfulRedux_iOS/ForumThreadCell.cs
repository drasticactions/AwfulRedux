using Foundation;
using System;
using UIKit;
using AwfulRedux.UI.Models.Threads;
using FFImageLoading;

namespace AwfulRedux_iOS
{
	public partial class ForumThreadCell : UITableViewCell
	{
		public ForumThreadCell (IntPtr handle) : base (handle)
		{
		}

		public void Update (Thread thread) 
		{
			// use FFImageLoading library to:
			ImageService.Instance
			            .LoadFileFromApplicationBundle(String.Format("{0}.png", thread.ImageIconLocation)) 	// get the image from the app bundle
			            .LoadingPlaceholder("missing.png") 						// specify a placeholder image									// transform the image to a circle
			            .Into(ThreadIcon);

			ThreadName.Text = thread.Name;
		}
	}
}