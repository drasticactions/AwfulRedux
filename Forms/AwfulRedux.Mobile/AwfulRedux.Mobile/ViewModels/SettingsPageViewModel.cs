using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Navigation;

namespace AwfulRedux.Mobile.ViewModels
{
    public class SettingsPageViewModel : BindableBase, INavigationAware
    {
        private INavigationService _navigationService;

        public DelegateCommand LoginCommand { get; }

        public SettingsPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            LoginCommand = new DelegateCommand(async () => await navigationService.NavigateAsync("LoginPage", useModalNavigation: true));
            UpdateViewModel();
        }

        private bool _isLoggedIn;

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set
            {
                _isLoggedIn = value;
                OnPropertyChanged();
            }
        }

        private string _userAvatar;

        public string UserAvatar
        {
            get { return _userAvatar; }
            set
            {
                _userAvatar = value;
                OnPropertyChanged();
            }
        }

        private string _loginUserName;

        public string LoginUserName
        {
            get { return _loginUserName; }
            set
            {
                _loginUserName = value;
                OnPropertyChanged();
            }
        }

        public void UpdateViewModel()
        {
            LoginUserName = App.DefaultUser == null ? "Dumb Dontrel" : App.DefaultUser.Username;
            UserAvatar = App.DefaultUser == null ? "dontrel.png" : App.DefaultUser.AvatarLink;
            IsLoggedIn = App.IsLoggedIn;
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            UpdateViewModel();
        }
    }
}
