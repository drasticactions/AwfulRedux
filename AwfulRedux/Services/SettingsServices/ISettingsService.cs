using System;
using Windows.UI.Xaml;

namespace AwfulRedux.Services.SettingsServices
{
    public interface ISettingsService
    {
        bool UseShellBackButton { get; set; }

        bool ShowEmbeddedTweets { get; set; }

        bool ShowEmbeddedVideo { get; set; }

        bool ShowEmbeddedGifv { get; set; }

        bool BackgroundEnable { get; set; }

        bool BookmarkBackground { get; set; }

        bool BookmarkNotifications { get; set; }

        DateTime LastRefresh { get; set; }

        ApplicationTheme AppTheme { get; set; }
        TimeSpan CacheMaxDuration { get; set; }
    }
}