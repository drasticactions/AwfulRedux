using System;
using System.Threading.Tasks;
using AwfulForumsLibrary.Models.Web;
using Newtonsoft.Json;
using UIKit;

namespace AwfulRedux_iOS
{
	public static class ResultChecker
	{
		public static async Task SendMessageDialogAsync(string message, bool isSuccess)
		{
			var title = isSuccess ? "AwfulRedux: " : "AwfulRedux - Error: ";
			new UIAlertView (title, message, null, "OK", null).Show();
		}

		public static async Task<bool> CheckPaywallOrSuccess(Result result, bool showMessage = true)
		{
			if (result.IsSuccess)
				return true;
			if (result.Type == typeof (Error).ToString())
			{
				var error = JsonConvert.DeserializeObject<Error>(result.ResultJson);
				if (error.IsPaywall)
				{
					return false;
				}
			}
			if (showMessage)
				await SendMessageDialogAsync(result.Type + Environment.NewLine + result.ResultJson + Environment.NewLine, false);
			return false;
		}

		public static async Task<bool> CheckSuccess(Result result, bool showMessage = true)
		{
			if (result.IsSuccess)
				return true;
			if (showMessage)
				await SendMessageDialogAsync(result.Type + Environment.NewLine + result.ResultJson + Environment.NewLine, false);
			return false;
		}
	}
}
