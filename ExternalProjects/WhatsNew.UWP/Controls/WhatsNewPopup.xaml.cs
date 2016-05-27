using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WhatsNew.UWP.Helpers;

namespace WhatsNew.UWP.Controls
{
    public sealed partial class WhatsNewPopup : UserControl
    {
        // Use this from XAML to control TxtTitle
        #region WhatsNewTitle Dependency Property

        public static readonly DependencyProperty WhatsNewTitleProperty =
            DependencyProperty.Register(
                "WhatsNewTitle", typeof(string), typeof(WhatsNewPopup),
                new PropertyMetadata(ResHelper.GetResource("WhatsNewTitle"), null));

        public static void SetWhatsNewTitle(WhatsNewPopup element, string value)
        {
            element.SetValue(WhatsNewTitleProperty, value);
        }

        public static string GetWhatsNewTitle(WhatsNewPopup element)
        {
            return (string)element.GetValue(WhatsNewTitleProperty);
        }

        #endregion

        // Use this from XAML to control application name 
        #region ApplicationName Dependency Property

        public static readonly DependencyProperty ApplicationNameProperty =
            DependencyProperty.Register(
                "ApplicationName", typeof(string), typeof(WhatsNewPopup),
                new PropertyMetadata(null, null));

        public static void SetApplicationName(WhatsNewPopup element, string value)
        {
            element.SetValue(ApplicationNameProperty, value);
        }

        public static string GetApplicationName(WhatsNewPopup element)
        {
            return (string)element.GetValue(ApplicationNameProperty);
        }

        #endregion

        // Use this from XAML to control rating WhatsNewMessage
        #region WhatsNewMessage Dependency Property

        public static readonly DependencyProperty WhatsNewMessageProperty =
            DependencyProperty.Register(
                "WhatsNewMessage", typeof(string), typeof(WhatsNewPopup),
                new PropertyMetadata(null, null));

        public static void SetWhatsNewMessage(WhatsNewPopup element, string value)
        {
            element.SetValue(WhatsNewMessageProperty, value);
        }

        public static string GetWhatsNewMessage(WhatsNewPopup element)
        {
            return (string)element.GetValue(WhatsNewMessageProperty);
        }

        #endregion

        // Use this from XAML to control overriding culture
        #region LanguageOverride Dependency Property

        public static readonly DependencyProperty LanguageOverrideProperty =
            DependencyProperty.Register("LanguageOverride", typeof(string), typeof(WhatsNewPopup), new PropertyMetadata(null, null));

        public static void SetLanguageOverride(WhatsNewPopup element, string value)
        {
            element.SetValue(LanguageOverrideProperty, value);
        }

        public static string GetLanguageOverride(WhatsNewPopup element)
        {
            return (string)element.GetValue(LanguageOverrideProperty);
        }

        #endregion

        // Use this from XAML to control WhatsNew button 
        #region WhatsNewYes Dependency Property

        public static readonly DependencyProperty WhatsNewYesProperty =
            DependencyProperty.Register(
                "WhatsNewYes", typeof(string), typeof(WhatsNewPopup),
                new PropertyMetadata(ResHelper.GetResource("WhatsNewYes"), null));

        public static void SetWhatsNewYes(WhatsNewPopup element, string value)
        {
            element.SetValue(WhatsNewYesProperty, value);
        }

        public static string GetWhatsNewYes(WhatsNewPopup element)
        {
            return (string)element.GetValue(WhatsNewYesProperty);
        }

        #endregion

        private string Title
        {
            set
            {
                if (TxtTitle.Text != value)
                {
                    TxtTitle.Text = value;
                }
            }
        }

        private string Message
        {
            set
            {
                if (TxtMessage.Text != value)
                {
                    TxtMessage.Text = value;
                }
            }
        }

        private string YesText
        {
            set
            {
                if (DigContent.PrimaryButtonText != value)
                {
                    DigContent.PrimaryButtonText = value;
                }
            }
        }

        public WhatsNewPopup()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            // WhatsNewTitle property is mandatory and must be defined in xaml.
            if (GetWhatsNewTitle(this) == null || GetWhatsNewTitle(this).Length <= 0)
            {
                throw new ArgumentNullException(nameof(WhatsNewTitleProperty), "Mandatory property not defined in WhatsNewPopup.");
            }

            // ApplicationName property is mandatory and must be defined in xaml.
            if (GetApplicationName(this) == null || GetApplicationName(this).Length <= 0)
            {
                throw new ArgumentNullException(nameof(ApplicationNameProperty), "Mandatory property not defined in WhatsNewPopup.");
            }

            // WhatsNewMessage property is mandatory and must be defined in xaml.
            if (GetWhatsNewMessage(this) == null || GetWhatsNewMessage(this).Length <= 0)
            {
                throw new ArgumentNullException(nameof(WhatsNewMessageProperty), "Mandatory property not defined in WhatsNewPopup.");
            }

            // Application language override.
            if (GetLanguageOverride(this) != null)
            {
                OverrideLanguage();
            }

            WhatsNewHelper.Default.Launching();

            TxtVersion.Text = WhatsNewHelper.Default.GetAppVersion();
            if (WhatsNewHelper.Default.State == WhatsNewState.VersionUpdate)
            {
                SetupMessage();
                ContentDialogResult result = await DigContent.ShowAsync();
            }
        }

        private void SetupMessage()
        {
            Title = string.Format(GetWhatsNewTitle(this), GetApplicationName());
            Message = GetWhatsNewMessage(this);
            YesText = GetWhatsNewYes(this);
        }

        /// <summary>
        /// Override default assembly dependent localization for the control
        /// with another culture supported by the application and the library.
        /// </summary>
        private void OverrideLanguage()
        {
            CultureInfo originalCulture = CultureInfo.DefaultThreadCurrentUICulture;
            CultureInfo newCulture = new CultureInfo(GetLanguageOverride(this));

            CultureInfo.DefaultThreadCurrentCulture = newCulture;
            CultureInfo.DefaultThreadCurrentUICulture = newCulture;

            SetWhatsNewTitle(this, string.Format(ResHelper.GetResource("WhatsNewTitle"), GetApplicationName()));
            SetWhatsNewYes(this, ResHelper.GetResource("WhatsNewYes"));

            CultureInfo.DefaultThreadCurrentCulture = originalCulture;
            CultureInfo.DefaultThreadCurrentUICulture = originalCulture;
        }

        /// <summary>
        /// Get application name.
        /// </summary>
        /// <returns>Name of the application.</returns>
        private string GetApplicationName()
        {
            string appName = GetApplicationName(this);

            // If application name has not been defined by the application,
            // extract it from the Application class.
            if (appName == null || appName.Length <= 0)
            {
                appName = Application.Current.ToString();
                if (appName.EndsWith(".App"))
                {
                    appName = appName.Remove(appName.LastIndexOf(".App"));
                }
            }

            return appName;
        }
    }
}
