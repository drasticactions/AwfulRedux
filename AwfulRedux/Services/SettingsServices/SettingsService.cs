using System;
using Windows.UI.Xaml;

namespace AwfulRedux.Services.SettingsServices
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-SettingsService
    public partial class SettingsService : ISettingsService
    {
        public static SettingsService Instance { get; }
        static SettingsService()
        {
            // implement singleton pattern
            Instance = Instance ?? new SettingsService();
        }

        readonly Template10.Services.SettingsService.ISettingsHelper _helper;
        private SettingsService()
        {
            _helper = new Template10.Services.SettingsService.SettingsHelper();
        }

        public bool UseShellBackButton
        {
            get { return _helper.Read<bool>(nameof(UseShellBackButton), true); }
            set
            {
                _helper.Write(nameof(UseShellBackButton), value);
                ApplyUseShellBackButton(value);
            }
        }

        public bool ShowEmbeddedTweets {
            get { return _helper.Read<bool>(nameof(ShowEmbeddedTweets), true); }
            set
            {
                _helper.Write(nameof(ShowEmbeddedTweets), value);
            }
        }
        public bool ShowEmbeddedVideo
        {
            get { return _helper.Read<bool>(nameof(ShowEmbeddedVideo), true); }
            set
            {
                _helper.Write(nameof(ShowEmbeddedVideo), value);
            }
        }
        public bool ShowEmbeddedGifv
        {
            get { return _helper.Read<bool>(nameof(ShowEmbeddedVideo), true); }
            set
            {
                _helper.Write(nameof(ShowEmbeddedVideo), value);
            }
        }

        public DateTime LastRefresh
        {
            get { return _helper.Read<DateTime>(nameof(LastRefresh), DateTime.Now); }
            set
            {
                _helper.Write(nameof(LastRefresh), value);
            }
        }

        public ApplicationTheme AppTheme
        {
            get
            {
                var theme = ApplicationTheme.Light;
                var value = _helper.Read<string>(nameof(AppTheme), theme.ToString());
                return Enum.TryParse<ApplicationTheme>(value, out theme) ? theme : ApplicationTheme.Light;
            }
            set
            {
                _helper.Write(nameof(AppTheme), value.ToString());
                ApplyAppTheme(value);
            }
        }

        public TimeSpan CacheMaxDuration
        {
            get { return _helper.Read<TimeSpan>(nameof(CacheMaxDuration), TimeSpan.FromDays(2)); }
            set
            {
                _helper.Write(nameof(CacheMaxDuration), value);
                ApplyCacheMaxDuration(value);
            }
        }
    }
}
