using Foundation;
using System;
using UIKit;
using AwfulForumsLibrary.Managers;

namespace AwfulRedux_iOS
{
	public partial class ForumTabBarViewController : UITabBarController
	{
		public ForumTabBarViewController (IntPtr handle) : base (handle)
		{
		}

		public async override void ViewDidLoad ()
		{
			//await AppDelegate.CheckLogIn();
			if (AppDelegate.WebManager == null) {
				AppDelegate.WebManager = await AuthDataSource.GetWebManager();
			}

			base.ViewDidLoad ();
		}
	}
}