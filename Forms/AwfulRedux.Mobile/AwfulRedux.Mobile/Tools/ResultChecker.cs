using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using AwfulRedux.Core.Models.Web;
using Newtonsoft.Json;

namespace AwfulRedux.Mobile.Tools
{
    public class ResultChecker
    {
        public static async Task<bool> CheckPaywallOrSuccess(Result result, bool showMessage = true)
        {
            if (result.IsSuccess)
                return true;
            if (result.Type == typeof(Error).ToString())
            {
                var error = JsonConvert.DeserializeObject<Error>(result.ResultJson);
                if (error.IsPaywall)
                {
                    return false;
                }
            }

            if (showMessage)
               await SendMessageDialogAsync (result.Type + Environment.NewLine + result.ResultJson + Environment.NewLine, false);
            return false;
        }

        public static async Task SendMessageDialogAsync(string message, bool isSuccess)
        {
            await UserDialogs.Instance.AlertAsync(message);
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
