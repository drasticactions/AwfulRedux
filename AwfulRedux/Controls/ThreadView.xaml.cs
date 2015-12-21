using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        public ThreadView()
        {
            this.InitializeComponent();
            ThreadFullView.NavigationCompleted += WebViewCommands.WebView_OnNavigationCompleted;
            ThreadFullView.ScriptNotify += WebViewCommands.WebViewNotifyCommand.WebView_ScriptNotify;
        }

        // strongly-typed view models enable x:bind
        public ThreadViewModel ViewModel => this.DataContext as ThreadViewModel;

        public async Task LoadThread(Thread thread)
        {
            ViewModel.Selected = thread;
            await ViewModel.LoadThread(thread);
        }
    }
}
