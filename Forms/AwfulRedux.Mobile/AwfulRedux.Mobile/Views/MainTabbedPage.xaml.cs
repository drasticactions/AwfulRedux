using System;
using Prism.Mvvm;
using Xamarin.Forms;

namespace AwfulRedux.Mobile.Views
{
    public partial class MainTabbedPage : TabbedPage
    {
        public MainTabbedPage()
        {
            InitializeComponent();
            this.CurrentPageChanged += OnCurrentPageChanged;
        }

        private async void OnCurrentPageChanged(object sender, EventArgs eventArgs)
        {
            //await App.LoginUser();
            var navPage = CurrentPage as NavigationPage;
            var settingsPage = navPage?.CurrentPage as SettingsPage;
            settingsPage?.UpdateViewModel();
        }
    }
}
