using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http.Filters;
using AwfulRedux.Core.Managers;
using AwfulRedux.Core.Tools;
using AwfulRedux.Database;
using AwfulRedux.Tools.Authentication;
using AwfulRedux.Tools.Database;
using AwfulRedux.Tools.Errors;
using AwfulRedux.UI.Models.Forums;
using AwfulRedux.UI.Models.Users;
using Newtonsoft.Json;
using SQLite.Net.Platform.WinRT;
using Template10.Mvvm;

namespace AwfulRedux.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        string _password = string.Empty;
        public string Password { get { return _password; } set { Set(ref _password, value); } }

        string _username = string.Empty;
        public string Username { get { return _username; } set { Set(ref _username, value); } }

        private bool _isLoggedIn = default(bool);

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set
            {
                Set(ref _isLoggedIn, value);
            }
        }

        private bool _isLoading = default(bool);

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                Set(ref _isLoading, value);
            }
        }

        private User _selectedUser = default(User);

        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                Set(ref _selectedUser, value);
            }
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            IsLoggedIn = Views.Shell.Instance.ViewModel.IsLoggedIn;
            if (IsLoggedIn)
            {
                var defaultUsers = await _db.GetAuthUsers();
                if (!defaultUsers.Any()) return;
                SelectedUser = defaultUsers.First();
            }
        }

        private readonly AuthenticationManager _authenticationManager = new AuthenticationManager();
        private readonly AuthenticatedUserDatabase _db = new AuthenticatedUserDatabase(new SQLitePlatformWinRT(), DatabaseWinRTHelpers.GetWinRTDatabasePath("ForumsRedux.db"));

        public async Task LogoutUser()
        {
            IsLoading = true;
            await _authenticationManager.LogoutAsync(Views.Shell.Instance.ViewModel.WebManager.AuthenticationCookie);
            await RemoveUserCookies();
            Views.Shell.Instance.ViewModel.IsLoggedIn = false;
            IsLoggedIn = false;
            await _db.RemoveUser(SelectedUser);
            SelectedUser = null;
            NavigationService.Navigate(typeof(Views.MainPage));
            IsLoading = false;
        }

        private async Task RemoveUserCookies()
        {
            var filter = new HttpBaseProtocolFilter();
            var cookieManager = filter.CookieManager;
            foreach (var cookie in cookieManager.GetCookies(new Uri("http://fake.forums.somethingawful.com")))
            {
                cookieManager.DeleteCookie(cookie);
            }
        }

        public async Task LoginUser()
        {
            IsLoading = true;
            await RemoveUserCookies();
            var result = await _authenticationManager.AuthenticateAsync(Username, Password);

            if (!result.IsSuccess)
            {
                await ResultChecker.SendMessageDialogAsync(result.Error, false);
                IsLoading = false;
                return;
            }
            
            Views.Shell.Instance.ViewModel.WebManager = new WebManager(result.AuthenticationCookie);

            var userManager = new UserManager(Views.Shell.Instance.ViewModel.WebManager);

            // 0 gets us the default user.
            var userResult = await userManager.GetUserFromProfilePage(0);
            var resultCheck = await ResultChecker.CheckSuccess(userResult);
            if (!resultCheck)
            {
                await ResultChecker.SendMessageDialogAsync("Failed to get user", false);
                IsLoading = false;
                return;
            }

            // Get the user

            try
            {
                var user = JsonConvert.DeserializeObject<User>(userResult.ResultJson);
                await _db.AddOrUpdateUser(user);
                resultCheck = await CookieManager.SaveCookie(user.Id + ".txt", result.AuthenticationCookie, new Uri(EndPoints.CookieDomainUrl));
            }
            catch (Exception)
            {
                resultCheck = false;
            }

            if (!resultCheck)
            {
                await ResultChecker.SendMessageDialogAsync("Failed to parse user", false);
                IsLoading = false;
                return;
            }

            // Update the main forum page
            //Views.Shell.ShowBusy(true, "Updating Forum List...");

            //var forumManager = new ForumManager(Views.Shell.Instance.WebManager);
            //var forumResult = await forumManager.GetForumCategoriesAsync();
            //resultCheck = await ResultChecker.CheckSuccess(forumResult);
            //if (!resultCheck)
            //{
            //    await ResultChecker.SendMessageDialogAsync("Failed to update initial forum list", false);
            //    Views.Shell.ShowBusy(false);
            //    return;
            //}

            //var forumCategoryEntities = JsonConvert.DeserializeObject<List<Category>>(forumResult.ResultJson);

            Views.Shell.Instance.ViewModel.IsLoggedIn = true;
            IsLoading = false;
            NavigationService.Navigate(typeof(Views.MainPage));
        }
    }
}
