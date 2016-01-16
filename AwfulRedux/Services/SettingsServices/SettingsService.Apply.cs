using System;
using Windows.ApplicationModel.Background;
using Windows.UI.Xaml;
using AwfulRedux.Tools.Background;

namespace AwfulRedux.Services.SettingsServices
{
    public partial class SettingsService
    {
        public void ApplyUseShellBackButton(bool value)
        {
            Template10.Common.BootStrapper.Current.NavigationService.Dispatcher.Dispatch(() =>
            {
                Template10.Common.BootStrapper.Current.ShowShellBackButton = value;
                Template10.Common.BootStrapper.Current.UpdateShellBackButton();
                Template10.Common.BootStrapper.Current.NavigationService.Refresh();
            });
        }

        public async void ChangeBackgroundStatus(bool value)
        {
            if (value)
            {
                var task = await
                        BackgroundTaskUtils.RegisterBackgroundTask(BackgroundTaskUtils.BackgroundTaskEntryPoint,
                            BackgroundTaskUtils.BackgroundTaskName,
                            new TimeTrigger(15, false),
                            null);
            }
            else
            {
                BackgroundTaskUtils.UnregisterBackgroundTasks(BackgroundTaskUtils.BackgroundTaskName);
            }
        }

        public void ApplyAppTheme(ApplicationTheme value)
        {
            Views.Shell.HamburgerMenu.RefreshStyles(value);
        }

        private void ApplyCacheMaxDuration(TimeSpan value)
        {
            Template10.Common.BootStrapper.Current.CacheMaxDuration = value;
        }
    }
}
