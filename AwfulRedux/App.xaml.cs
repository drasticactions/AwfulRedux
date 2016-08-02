using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Metadata;
using Windows.Media.SpeechRecognition;
using Windows.Storage;
using Windows.System.Profile;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AwfulRedux.Database;
using AwfulRedux.Services.SettingsServices;
using AwfulRedux.Tools.Background;
using AwfulRedux.Tools.Database;
using AwfulRedux.Tools.Helper;
using AwfulRedux.Views;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using SQLite.Net.Platform.WinRT;
using Template10.Common;
using Template10.Controls;

namespace AwfulRedux
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki

    sealed partial class App : Template10.Common.BootStrapper
    {
        public static ISettingsService Settings;

        public static Random Random = new Random();

        public App()
        {
            InitializeComponent();
            if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Xbox")
            {
                this.RequiresPointerMode = Windows.UI.Xaml.ApplicationRequiresPointerMode.WhenRequested;
            }
            #region App settings

            Settings = SettingsService.Instance;
            RequestedTheme = Settings.AppTheme;

            #endregion 
            #region Database
            var db = new Database.DataSource.MainForums(new SQLitePlatformWinRT(), DatabaseWinRTHelpers.GetWinRTDatabasePath("ForumsRedux.db"));
            db.CreateDatabase();
            var bdb = new Database.DataSource.Bookmarks(new SQLitePlatformWinRT(), DatabaseWinRTHelpers.GetWinRTDatabasePath("BookmarkRedux.db"));
            db.CreateDatabase();
            bdb.CreateDatabase();
            #endregion
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            if ((Window.Current.Content as ModalDialog) != null)
            {
                var content = (ModalDialog) Window.Current.Content;
                var shell = content.Content as Shell;
                if (shell != null)
                {
                    await shell.ViewModel.LoginUser();
                }
            }
            if (startKind == StartKind.Activate)
            {
                if (args.Kind == ActivationKind.ToastNotification)
                {
                    //Get the pre-defined arguments and user inputs from the eventargs;
                    var toastArgs = args as ToastNotificationActivatedEventArgs;
                    if (toastArgs == null)
                        return;
                    var arguments = JsonConvert.DeserializeObject<ToastNotificationArgs>(toastArgs.Argument);
                    if (arguments != null && arguments.threadId > 0)
                    {
                        NavigationService.Navigate(typeof (Views.BookmarksPage), arguments.threadId);
                    }
                }
                else if (args.Kind == ActivationKind.VoiceCommand)
                {
                    if (await Views.Shell.Instance.ViewModel.HasLogin())
                    {
                        var commandArgs = args as VoiceCommandActivatedEventArgs;
                        HandleVoiceRequest(commandArgs);
                    }
                }
            }
            else
            {
                var launch = args as LaunchActivatedEventArgs;
                if (launch?.PreviousExecutionState == ApplicationExecutionState.NotRunning 
                    || launch?.PreviousExecutionState == ApplicationExecutionState.Terminated
                    || launch?.PreviousExecutionState == ApplicationExecutionState.ClosedByUser)
                {
                    NavigationService.Navigate(typeof (Views.MainPage));
                }
            }
            try
            {
                // Install the main VCD. Since there's no simple way to test that the VCD has been imported, or that it's your most recent
                // version, it's not unreasonable to do this upon app load.
                StorageFile vcdStorageFile = await Package.Current.InstalledLocation.GetFileAsync(@"SomethingAwfulCommands.xml");

                await Windows.ApplicationModel.VoiceCommands.VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(vcdStorageFile);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Installing Voice Commands Failed: " + ex.ToString());
            }

            if (Windows.Foundation.Metadata.ApiInformation
            .IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                Windows.UI.ViewManagement.StatusBar.GetForCurrentView().BackgroundOpacity = 1;
            }

            await Task.CompletedTask;
        }

        public override async void OnResuming(object s, object e, AppExecutionState previousExecutionState)
        {
            base.OnResuming(s, e, previousExecutionState);
            // On Restore, if we have a frame, remake navigation so we can go back to previous pages.
            try
            {
                var app = s as App;

                var page = app?.NavigationService.Frame.Content as BookmarksPage;
                if (page != null)
                {
                    Current.NavigationService.FrameFacade.BackRequested +=
                        page.ViewModel.MasterDetailViewControl.NavigationManager_BackRequested;
                }
                else
                {
                    var threadpage = app?.NavigationService.Frame.Content as ThreadListPage;
                    if (threadpage != null)
                    {
                        Current.NavigationService.FrameFacade.BackRequested += page.ViewModel.MasterDetailViewControl.NavigationManager_BackRequested;
                    }
                }
            }
            catch (Exception ex)
            {
                // Ignore, continue.
            }

            if (Windows.Foundation.Metadata.ApiInformation
            .IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                Windows.UI.ViewManagement.StatusBar.GetForCurrentView().BackgroundOpacity = 1;
            }

            await Task.CompletedTask;
        }

        // runs even if restored from state
        public override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            // Setup Background
            var isIoT = ApiInformation.IsTypePresent("Windows.Devices.Gpio.GpioController");

            if (!isIoT)
            {
                TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
                BackgroundTaskUtils.UnregisterBackgroundTasks(BackgroundTaskUtils.ToastBackgroundTaskName);
                var task2 = await
                    BackgroundTaskUtils.RegisterBackgroundTask(BackgroundTaskUtils.ToastBackgroundTaskEntryPoint,
                        BackgroundTaskUtils.ToastBackgroundTaskName, new ToastNotificationActionTrigger(),
                        null);

                if (Settings.BackgroundEnable)
                {
                    BackgroundTaskUtils.UnregisterBackgroundTasks(BackgroundTaskUtils.BackgroundTaskName);
                    var task = await
                        BackgroundTaskUtils.RegisterBackgroundTask(BackgroundTaskUtils.BackgroundTaskEntryPoint,
                            BackgroundTaskUtils.BackgroundTaskName,
                            new TimeTrigger(15, false),
                            null);
                }
            }

            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationViewBoundsMode") && AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Xbox")
            {
                var AppView = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
                AppView.SetDesiredBoundsMode(Windows.UI.ViewManagement.ApplicationViewBoundsMode.UseCoreWindow);
            }

            var launch = args as LaunchActivatedEventArgs;
            if (launch?.PreviousExecutionState == ApplicationExecutionState.NotRunning 
                || launch?.PreviousExecutionState == ApplicationExecutionState.Terminated
                || launch?.PreviousExecutionState == ApplicationExecutionState.ClosedByUser)
            {
                // setup hamburger shell
                // content may already be shell when resuming
                if ((Window.Current.Content as ModalDialog) == null)
                {
                    // setup hamburger shell inside a modal dialog
                    var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);
                    Window.Current.Content = new ModalDialog
                    {
                        DisableBackButtonWhenModal = true,
                        Content = new Views.Shell(nav),
                        ModalContent = new Views.Busy(),
                    };
                    var content = (ModalDialog)Window.Current.Content;
                    var shell = content.Content as Shell;
                    if (shell != null)
                    {
                        await shell.ViewModel.LoginUser();
                    }
                }
            }

            await Task.CompletedTask;
        }

        private string SemanticInterpretation(string interpretationKey, SpeechRecognitionResult speechRecognitionResult)
        {
            return speechRecognitionResult.SemanticInterpretation.Properties[interpretationKey].FirstOrDefault();
        }

        public void HandleVoiceRequest(VoiceCommandActivatedEventArgs commandArgs)
        {
            Windows.Media.SpeechRecognition.SpeechRecognitionResult speechRecognitionResult = commandArgs.Result;

            // Get the name of the voice command and the text spoken. See AdventureWorksCommands.xml for
            // the <Command> tags this can be filled with.
            string voiceCommandName = speechRecognitionResult.RulePath[0];
            string textSpoken = speechRecognitionResult.Text;

            // The commandMode is either "voice" or "text", and it indictes how the voice command
            // was entered by the user.
            // Apps should respect "text" mode by providing feedback in silent form.
            string commandMode = this.SemanticInterpretation("commandMode", speechRecognitionResult);

            switch (voiceCommandName)
            {
                case "openBookmarks":
                    NavigationService.Navigate(typeof(Views.BookmarksPage));
                    break;
                case "openPrivateMessages":
                    NavigationService.Navigate(typeof(Views.PrivateMessageListPage));
                    break;
                case "lowtaxIsAJerk":
                    // TODO: Maybe fix this? Not like anyone would care.
                    NavigationService.Navigate(typeof(Views.BookmarksPage));
                    break;
                default:
                    break;
            }
        }
    }
}

