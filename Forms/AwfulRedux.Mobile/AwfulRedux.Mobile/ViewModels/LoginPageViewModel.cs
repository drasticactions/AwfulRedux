using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AwfulRedux.Core.Managers;
using AwfulRedux.Core.Tools;
using AwfulRedux.Database;
using AwfulRedux.Mobile.Tools;
using AwfulRedux.UI.Models.Users;
using Newtonsoft.Json;
using Prism.Navigation;
using Xamarin.Forms;

namespace AwfulRedux.Mobile.ViewModels
{
    public class LoginPageViewModel : BindableBase, INavigationAware
    {
        private INavigationService _navigationService;

        public LoginPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            GoBackCommand = new DelegateCommand(async () => await navigationService.GoBackAsync());
        }

        public string ValidationErrors { get; private set; }

        public DelegateCommand GoBackCommand { get; }

        private readonly LocalStorageManager _localStorageManager = new LocalStorageManager();
        private readonly AuthenticationManager _authenticationManager = new AuthenticationManager();
        private readonly AuthenticatedUserDatabase _db = new AuthenticatedUserDatabase(DependencyService.Get<ISQLite>().GetPlatform(), DependencyService.Get<ISQLite>().GetPath("ForumsRedux.db"));

        public bool CanLogin()
        {
            ValidationErrors = "";
            if (string.IsNullOrEmpty(Username))
            {
                ValidationErrors = "Please enter a username.";
            }
            if (string.IsNullOrEmpty(Password))
            {
                ValidationErrors += "Please enter a password.";
            }
            return (ValidationErrors == "");
        }

        public async Task NavigateToSettings()
        {
            await _navigationService.GoBackAsync();
        }

        public async Task<bool> Login()
        {
            var result = await _authenticationManager.AuthenticateAsync(Username, Password);
            if (!result.IsSuccess)
            {
                ValidationErrors = "Bad Username or Password.";
                return false;
            }
            App.WebManager = new WebManager(result.AuthenticationCookie);
            var userManager = new UserManager(App.WebManager);
            var userResult = await userManager.GetUserFromProfilePage(0);
            if (userResult == null)
            {
                ValidationErrors = "Failed to get a user";
                return false;
            }
            var user = JsonConvert.DeserializeObject<User>(userResult.ResultJson);
            await _db.AddOrUpdateUser(user);
            await _localStorageManager.SaveCookie(user.Id + ".txt", result.AuthenticationCookie, new Uri(EndPoints.CookieDomainUrl));
            var cookieContainer = await _localStorageManager.LoadCookie(user.Id + ".txt");
            if (cookieContainer.Count == 0)
            {
                ValidationErrors = "Failed to save cookies";
                return false;
            }
            return true;
        }

        private string _username;

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }


        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {

        }
    }
}
