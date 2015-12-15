using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AwfulRedux.UI.Models.Forums;
using AwfulRedux.ViewModels;
using Template10.Services.NavigationService;

namespace AwfulRedux.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // strongly-typed view models enable x:bind
        public MainPageViewModel ViewModel => this.DataContext as MainPageViewModel;

        private void MainForumListFull_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var forum = e.ClickedItem as Forum;
            ViewModel.NavigateToThreadList(forum);
        }
    }
}
