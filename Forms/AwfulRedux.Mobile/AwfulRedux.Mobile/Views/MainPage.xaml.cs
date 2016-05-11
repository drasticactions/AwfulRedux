using AwfulRedux.Mobile.ViewModels;
using Xamarin.Forms;

namespace AwfulRedux.Mobile.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            var vm = (MainPageViewModel)BindingContext;
            listView.RefreshCommand = new Command(async () =>
            {
                listView.IsRefreshing = true;
                await vm.Initialize(true);
                listView.IsRefreshing = false;
            });
        }
    }
}
