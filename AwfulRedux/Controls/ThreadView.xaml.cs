using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AwfulRedux.Tools.Web;
using AwfulRedux.UI.Models.Threads;
using AwfulRedux.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AwfulRedux.Controls
{
    public sealed partial class ThreadView : UserControl
    {
        public static ThreadView Instance { get; set; }
        public ThreadView()
        {
            this.InitializeComponent();
            ThreadFullView.NavigationCompleted += WebViewCommands.WebView_OnNavigationCompleted;
            ThreadFullView.ScriptNotify += WebViewCommands.WebViewNotifyCommand.WebView_ScriptNotify;
            if(PageNumberButton2.Flyout != null)
                PageNumberButton2.Flyout.Opened += FlyoutOnOpened;
            Instance = this;
        }

        private void FlyoutOnOpened(object sender, object o)
        {
            PageNumberTextBox.Focus(FocusState.Programmatic);
        }

        // strongly-typed view models enable x:bind
        public ThreadViewModel ViewModel => this.DataContext as ThreadViewModel;

        public async Task LoadThread(Thread thread, bool fromSuspend = false, bool lastPage = false)
        {
            if (lastPage)
            {
                thread.CurrentPage = thread.TotalPages;
                thread.RepliesSinceLastOpened = 0;
            }
            else if (thread.CurrentPage == 0)
            {
                thread.CurrentPage = 1;
            }
            if (fromSuspend && ViewModel.Selected != null)
            {
                return;
            }
            ViewModel.Selected = thread;
            await ViewModel.LoadThread();
        }

        private async void ScrollToBottom(object sender, RoutedEventArgs e)
        {
            await ThreadFullView.InvokeScriptAsync("ScrollToBottom", null);
        }

        private void PageNumberTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            int n;
            bool isNumeric = int.TryParse(PageNumberTextBox.Text, out n);
            if (isNumeric)
            {
                ViewModel.PageSelection = PageNumberTextBox.Text;
            }
        }
    }
}
