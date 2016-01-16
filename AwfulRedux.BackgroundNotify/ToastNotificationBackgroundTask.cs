using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.UI.Notifications;
using Microsoft.VisualBasic;
using Template10.Services.SettingsService;

namespace AwfulRedux.BackgroundNotify
{
    public sealed class ToastNotificationBackgroundTask : IBackgroundTask
    {

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            Template10.Services.SettingsService.ISettingsHelper _helper = new SettingsHelper();

            var details = taskInstance.TriggerDetails as ToastNotificationActionTriggerDetail;
            var arguments = details?.Argument;
            if (arguments == "sleep")
            {
                _helper.Write("BookmarkNotifications", false);
            }
            deferral.Complete();
        }
    }
}
