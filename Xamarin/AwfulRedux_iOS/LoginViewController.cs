using Foundation;
using System;
using UIKit;
using System.Threading;
using System.Threading.Tasks;
using AwfulForumsLibrary.Managers;
using Newtonsoft.Json;
using AwfulRedux.UI.Models.Users;

namespace AwfulRedux_iOS
{
	public partial class LoginViewController : UIViewController
	{
		public event EventHandler OnLoginSuccess;

		private readonly AuthenticationManager _authenticationManager = new AuthenticationManager();

		public LoginViewController (IntPtr handle) : base (handle)
		{
		}

		LoadingOverlay loadingOverlay;

		async partial void LoginButton_TouchUpInside (UIButton sender)
		{
			//Validate our Username & Password.
			//This is usually a web service call.
			if(IsUserNameValid () && IsPasswordValid ())
			{
				var bounds = UIScreen.MainScreen.Bounds;

				// show the loading overlay on the UI thread using the correct orientation sizing
				loadingOverlay = new LoadingOverlay (bounds);
				View.Add (loadingOverlay);
				var result = await _authenticationManager.AuthenticateAsync(UsernameField.Text, PasswordField.Text);

				if (!result.IsSuccess)
				{
					loadingOverlay.Hide();
					new UIAlertView ("Login Error", result.Error, null, "OK", null).Show();
					return;
				}

				var webManager = new WebManager(result.AuthenticationCookie);
				var userManager = new UserManager(webManager);

				var userResult = await userManager.GetUserFromProfilePage(0);
				var resultCheck = await ResultChecker.CheckSuccess(userResult);
				if (!resultCheck)
				{
					loadingOverlay.Hide();
					new UIAlertView ("Login Error", "Failed to get user", null, "OK", null).Show();
					return;
				}

				// Get the user

				try
				{
					var user = JsonConvert.DeserializeObject<User>(userResult.ResultJson);
					resultCheck = await AuthDataSource.SaveUser(user, result.AuthenticationCookie);
				}
				catch (Exception)
				{
					resultCheck = false;
				}

				if (!resultCheck)
				{
					loadingOverlay.Hide();
					new UIAlertView ("Login Error", "Failed to parse and save user", null, "OK", null).Show();
					return;
				}

				loadingOverlay.Hide();
				//We have successfully authenticated a the user,
				//Now fire our OnLoginSuccess Event.
				if (OnLoginSuccess != null)
				{
					OnLoginSuccess (sender, new EventArgs ());
				}
			}
			else
			{
				new UIAlertView ("Login Error", "Bad user name or password", null, "OK", null).Show();
			}
		}

		bool IsUserNameValid()
		{
			return !String.IsNullOrEmpty(UsernameField.Text.Trim());
		}

		bool IsPasswordValid()
		{
			return !String.IsNullOrEmpty(PasswordField.Text.Trim());
		}
	}
}