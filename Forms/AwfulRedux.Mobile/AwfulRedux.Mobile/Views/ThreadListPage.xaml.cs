using System.Collections.ObjectModel;
using AwfulRedux.Mobile.ViewModels;
using AwfulRedux.UI.Models.Threads;
using Xamarin.Forms;

namespace AwfulRedux.Mobile.Views
{
    public partial class ThreadListPage : ContentPage
    {
        public ThreadListPage()
        {
            InitializeComponent();
            var vm = (ThreadListPageViewModel)BindingContext;
            listView.LoadMoreCommand = new Command(async () => await vm.GetThreads());
            listView.RefreshCommand = new Command(async () =>
            {
                listView.IsRefreshing = true;
                vm.PageCount = 1;
                vm.Threads = new ObservableCollection<Thread>();
                await vm.GetThreads();
                listView.IsRefreshing = false;
            });
        }
    }
}
