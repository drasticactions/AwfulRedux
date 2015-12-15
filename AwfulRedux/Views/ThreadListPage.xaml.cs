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

namespace AwfulRedux.Views
{
    public sealed partial class ThreadListPage : Page
    {
        public ThreadListPage()
        {
            this.InitializeComponent();
        }

        // strongly-typed view models enable x:bind
        public ThreadListPageViewModel ViewModel => this.DataContext as ThreadListPageViewModel;

        private async void ThreadList_OnClick(object sender, ItemClickEventArgs e)
        {
            await ViewModel.LoadThread(e.ClickedItem as Thread);
        }
    }
}
