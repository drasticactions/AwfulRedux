using System.Linq;
using System.Threading.Tasks;
using AwfulRedux.Core.Managers;
using AwfulRedux.Database;
using AwfulRedux.Mobile.Tools;
using Prism.Unity;
using AwfulRedux.Mobile.Views;
using AwfulRedux.UI.Models.Users;
using Xamarin.Forms;

namespace AwfulRedux.Mobile
{
    public partial class App : PrismApplication
    {
        #region Helpers
        public static WebManager WebManager { get; set; } = new WebManager();

        public static bool IsLoggedIn { get; set; }

        public static User DefaultUser { get; set; }

        public static async Task LoginUser()
        {
            var udb = new AuthenticatedUserDatabase(DependencyService.Get<ISQLite>().GetPlatform(), DependencyService.Get<ISQLite>().GetPath("ForumsRedux.db"));
            var defaultUsers = await udb.GetAuthUsers();
            if (!defaultUsers.Any()) return;
            DefaultUser = defaultUsers.First();
            var localStorageManager = new LocalStorageManager();
            var cookie = await localStorageManager.LoadCookie(DefaultUser.Id + ".txt");
            WebManager = new WebManager(cookie);
            IsLoggedIn = true;
        }
        #endregion

        protected override async void OnInitialized()
        {
            InitializeComponent();

            #region Database
            var db = new Database.DataSource.MainForums(DependencyService.Get<ISQLite>().GetPlatform(), DependencyService.Get<ISQLite>().GetPath("ForumsRedux.db"));
            db.CreateDatabase();
            var bdb = new Database.DataSource.Bookmarks(DependencyService.Get<ISQLite>().GetPlatform(), DependencyService.Get<ISQLite>().GetPath("BookmarkRedux.db"));
            db.CreateDatabase();
            bdb.CreateDatabase();
            #endregion
            
            await NavigationService.NavigateAsync("MainTabbedPage");
            await LoginUser();

        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<MainTabbedPage>();
            Container.RegisterTypeForNavigation<MainPage>();
            Container.RegisterTypeForNavigation<ThreadListPage>();
            Container.RegisterTypeForNavigation<SettingsPage>();
            Container.RegisterTypeForNavigation<LoginPage>();
        }
    }
}
