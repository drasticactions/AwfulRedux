using System;
using System.Collections.Generic;
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
using AwfulRedux.UI.Models.Threads;
using AwfulRedux.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AwfulRedux.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BookmarksPage : Page
    {
        public BookmarksPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        private async void RefreshAction()
        {
            await ViewModel.Refresh();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Template10.Common.BootStrapper.Current.NavigationService.FrameFacade.BackRequested += previewControl.NavigationManager_BackRequested;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            Template10.Common.BootStrapper.Current.NavigationService.FrameFacade.BackRequested -= previewControl.NavigationManager_BackRequested;
            if (e.NavigationMode == NavigationMode.Back)
            {
                ResetPageCache();
            }
        }

        private void ResetPageCache()
        {
            var cacheSize = ((Frame)Parent).CacheSize;
            ((Frame)Parent).CacheSize = 0;
            ((Frame)Parent).CacheSize = cacheSize;
        }

        // strongly-typed view models enable x:bind
        public BookmarkViewModel ViewModel => this.DataContext as BookmarkViewModel;

        private async void MasterListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (masterListBox.SelectedItem == null) return;
            var thread = masterListBox.SelectedItem as Thread;
            await ThreadPageView.LoadThread(thread);
        }
    }
}
