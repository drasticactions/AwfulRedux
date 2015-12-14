using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private readonly AuthenticationManager _authenticationManager = new AuthenticationManager();
        private readonly AuthenticatedUserDatabase _db = new AuthenticatedUserDatabase(new SQLitePlatformWinRT(), DatabaseWinRTHelpers.GetWinRTDatabasePath("Forums.db"));
        public async Task LoginUser()
        {
            Views.Shell.ShowBusy(true, "Logging In...");
            var result = await _authenticationManager.AuthenticateAsync(Username, Password);

            if (!result.IsSuccess)
            {
                await ResultChecker.SendMessageDialogAsync(result.Error, false);
                Views.Shell.ShowBusy(false);
                return;
            }
            
            Views.Shell.Instance.WebManager = new WebManager(result.AuthenticationCookie);
            Views.Shell.ShowBusy(true, "Getting User Information...");

            var userManager = new UserManager(Views.Shell.Instance.WebManager);

            // 0 gets us the default user.
            var userResult = await userManager.GetUserFromProfilePage(0);
            var resultCheck = await ResultChecker.CheckSuccess(userResult);
            if (!resultCheck)
            {
                await ResultChecker.SendMessageDialogAsync("Failed to get user", false);
                Views.Shell.ShowBusy(false);
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
                Views.Shell.ShowBusy(false);
                return;
            }

            // Update the main forum page
            Views.Shell.ShowBusy(true, "Updating Forum List...");

            var forumManager = new ForumManager(Views.Shell.Instance.WebManager);
            var forumResult = await forumManager.GetForumCategoriesAsync();
            resultCheck = await ResultChecker.CheckSuccess(forumResult);
            if (!resultCheck)
            {
                await ResultChecker.SendMessageDialogAsync("Failed to update initial forum list", false);
                Views.Shell.ShowBusy(false);
                return;
            }

            var forumCategoryEntities = JsonConvert.DeserializeObject<List<Category>>(forumResult.ResultJson);


            NavigationService.Navigate(typeof(Views.MainPage));
            Views.Shell.ShowBusy(false);

        }
    }
}
