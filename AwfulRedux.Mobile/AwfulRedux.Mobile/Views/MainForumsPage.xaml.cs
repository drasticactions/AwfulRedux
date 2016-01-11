using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulRedux.Mobile.ViewModel;
using Xamarin.Forms;

namespace AwfulRedux.Mobile.Views
{
    public partial class MainForumsPage : ContentPage
    {
        private MainForumsPageViewModel _vm;
        public MainForumsPage()
        {
            InitializeComponent();
            _vm = BindingContext as MainForumsPageViewModel;
            _vm.Initialize();
        }
    }
}
