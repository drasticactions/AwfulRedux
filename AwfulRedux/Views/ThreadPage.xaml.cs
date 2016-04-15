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
using Newtonsoft.Json;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AwfulRedux.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ThreadPage : Page
    {
        public ThreadPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            ViewModel.ThreadView = this.ThreadPageView;
        }

        public ThreadPageViewModel ViewModel => this.DataContext as ThreadPageViewModel;

        //protected override async void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    base.OnNavigatedTo(e);
        //    if (e.Parameter == null) return;
        //    var thread = JsonConvert.DeserializeObject<Thread>(e.Parameter.ToString());
        //    ThreadPageView.ViewModel.Selected = thread;
        //    await ThreadPageView.ViewModel.LoadThread();
        //}
    }
}
