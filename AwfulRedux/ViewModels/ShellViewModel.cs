using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Navigation;
using AwfulForumsLibrary.Managers;
using AwfulRedux.Database;
using AwfulRedux.Tools.Authentication;
using AwfulRedux.Tools.Database;
using AwfulRedux.UI.Models.Threads;
using AwfulRedux.Views;
using Newtonsoft.Json;
using SQLite.Net.Platform.WinRT;
using Template10.Common;
using Template10.Mvvm;

namespace AwfulRedux.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        public WebManager WebManager { get; set; }

        private bool _isLoggedIn = default(bool);

        private readonly AuthenticatedUserDatabase _udb = new AuthenticatedUserDatabase(new SQLitePlatformWinRT(), DatabaseWinRTHelpers.GetWinRTDatabasePath("ForumsRedux.db"));
        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set
            {
                Set(ref _isLoggedIn, value);
            }
        }

        public Color AccentColor => (Color) BootStrapper.Current.Resources["SystemAccentColor"];

        public async Task<bool> HasLogin()
        {
            var defaultUsers = await _udb.GetAuthUsers();
            return defaultUsers.Any();
        }

        public async Task LoginUser()
        {
            var defaultUsers = await _udb.GetAuthUsers();
            if (!defaultUsers.Any()) return;
            var defaultUser = defaultUsers.First();
            var cookie = await CookieManager.LoadCookie(defaultUser.Id + ".txt");
            WebManager = new WebManager(cookie);
            IsLoggedIn = true;
        }
    }
}
