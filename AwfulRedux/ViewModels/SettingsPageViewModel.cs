using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Template10.Mvvm;

namespace AwfulRedux.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        public SettingsPartViewModel SettingsPartViewModel { get; } = new SettingsPartViewModel();
        public AboutPartViewModel AboutPartViewModel { get; } = new AboutPartViewModel();
    }

    public class SettingsPartViewModel : ViewModelBase
    {
        readonly Services.SettingsServices.SettingsService _settings;

        public SettingsPartViewModel()
        {
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                _settings = Services.SettingsServices.SettingsService.Instance;
        }

        public bool UseShowEmbeddedGifvButton
        {
            get { return _settings.ShowEmbeddedGifv; }
            set { _settings.ShowEmbeddedGifv = value; base.RaisePropertyChanged(); }
        }

        public bool UseShowEmbeddedTweetsButton
        {
            get { return _settings.ShowEmbeddedTweets; }
            set { _settings.ShowEmbeddedTweets = value; base.RaisePropertyChanged(); }
        }

        public bool UseShowEmbeddedVideoButton
        {
            get { return _settings.ShowEmbeddedVideo; }
            set { _settings.ShowEmbeddedVideo = value; base.RaisePropertyChanged(); }
        }

        public bool UseDarkThemeButton
        {
            get { return _settings.AppTheme.Equals(ApplicationTheme.Dark); }
            set { _settings.AppTheme = value ? ApplicationTheme.Dark : ApplicationTheme.Light; base.RaisePropertyChanged(); }
        }
    }

    public class AboutPartViewModel : ViewModelBase
    {
        public Uri Logo => Windows.ApplicationModel.Package.Current.Logo;

        public string DisplayName => Windows.ApplicationModel.Package.Current.DisplayName;

        public string Publisher => Windows.ApplicationModel.Package.Current.PublisherDisplayName;

        public string Version
        {
            get
            {
                var ver = Windows.ApplicationModel.Package.Current.Id.Version;
                return ver.Major.ToString() + "." + ver.Minor.ToString() + "." + ver.Build.ToString() + "." + ver.Revision.ToString();
            }
        }

        public Uri RateMe => new Uri("http://bing.com");
    }
}
