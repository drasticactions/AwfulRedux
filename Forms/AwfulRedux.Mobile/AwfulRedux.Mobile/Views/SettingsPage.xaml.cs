using AwfulRedux.Mobile.ViewModels;
using Xamarin.Forms;

namespace AwfulRedux.Mobile.Views
{
    public partial class SettingsPage : ContentPage
    {
        private SettingsPageViewModel _vm;
        public SettingsPage()
        {
            InitializeComponent();
            _vm = (SettingsPageViewModel) BindingContext;
        }

        public void UpdateViewModel()
        {
            _vm.UpdateViewModel();
        }
    }
}
