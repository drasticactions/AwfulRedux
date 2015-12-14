using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
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
        ISettingsService _settings;

        public App()
        {
            InitializeComponent();

            #region App settings

            _settings = SettingsService.Instance;
            RequestedTheme = _settings.AppTheme;

            #endregion

            #region Database
            var db = new Database.DataSource.MainForums(new SQLitePlatformWinRT(), DatabaseWinRTHelpers.GetWinRTDatabasePath("Forums.db"));
            db.CreateDatabase();
            #endregion
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            NavigationService.Navigate(typeof(Views.MainPage));
            await Task.CompletedTask;
        }

        // runs even if restored from state
        public override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            // setup hamburger shell
            var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);
            Window.Current.Content = new Shell(nav);
            await Task.CompletedTask;
        }
    }
}

