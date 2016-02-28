using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulRedux.UI.Models.Forums;
using AwfulRedux.Mobile.ViewModel;
using Xamarin.Forms;

namespace AwfulRedux.Mobile.Views
{
    public partial class MainForumsPage : ContentPage
    {
        private MainForumsPageViewModel _vm;
        public Action<Forum> ItemSelected { get; set; }
        public MainForumsPage()
        {
            InitializeComponent();
            BindingContext = new MainForumsPageViewModel(this);
            _vm = (MainForumsPageViewModel) BindingContext;
            _vm.ItemSelected = ItemSelected;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_vm.ForumCategories?.Count > 0 || _vm.IsBusy)
                return;

            await _vm.Initialize();
        }
    }
}
