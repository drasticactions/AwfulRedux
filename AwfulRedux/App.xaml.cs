using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AwfulRedux.Database;
using AwfulRedux.Services.SettingsServices;
using AwfulRedux.Tools.Database;
using AwfulRedux.Views;
using SQLite.Net.Platform.WinRT;

namespace AwfulRedux
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki

    sealed partial class App : Template10.Common.BootStrapper
    {
        public static ISettingsService Settings;

        public static Frame Frame;
        public App()
        {
            InitializeComponent();

            #region App settings

            Settings = SettingsService.Instance;
            RequestedTheme = Settings.AppTheme;

            #endregion

            #region Database
            var db = new Database.DataSource.MainForums(new SQLitePlatformWinRT(), DatabaseWinRTHelpers.GetWinRTDatabasePath("Forums.db"));
            db.CreateDatabase();
            var bdb = new Database.DataSource.Bookmarks(new SQLitePlatformWinRT(), DatabaseWinRTHelpers.GetWinRTDatabasePath("Bookmark.db"));
            db.CreateDatabase();
            bdb.CreateDatabase();
            #endregion
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            NavigationService.Navigate(typeof(Views.MainPage));
            await Task.CompletedTask;
        }

        public override async void OnResuming(object s, object e)
        {
            base.OnResuming(s, e);
            // On Restore, if we have a frame, remake navigation so we can go back to previous pages.
            if (Frame != null)
            {
                var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include, Frame);
                var shell = (Shell)Window.Current.Content;
                await shell.ViewModel.LoginUser();
                shell.SetNav(nav);
            }
            await Task.CompletedTask;
        }

        // runs even if restored from state
        public override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            // setup hamburger shell
            Frame = new Frame();
            var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include, Frame);
            var shell = new Shell(nav);
            await shell.ViewModel.LoginUser();
            Window.Current.Content = shell;
            await Task.CompletedTask;
        }
    }
}

