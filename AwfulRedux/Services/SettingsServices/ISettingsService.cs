using System;
using Windows.UI.Xaml;

namespace AwfulRedux.Services.SettingsServices
{
    public interface ISettingsService
    {
        bool UseShellBackButton { get; set; }

        bool ShowEmbeddedTweets { get; set; }

        bool OpenThreadsInNewWindow { get; set; }

        bool ShowEmbeddedVideo { get; set; }

        bool ShowEmbeddedGifv { get; set; }

        bool BackgroundEnable { get; set; }

        bool TransparentThreadListBackground { get; set; }

        bool BookmarkBackground { get; set; }

        bool BookmarkNotifications { get; set; }

        bool ImgurSignedIn { get; set; }

        string ImgurUsername { get; set; }

        DateTime LastRefresh { get; set; }

        ApplicationTheme AppTheme { get; set; }
        TimeSpan CacheMaxDuration { get; set; }
    }
}