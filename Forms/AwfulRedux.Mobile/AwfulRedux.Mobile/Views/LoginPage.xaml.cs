using System;
using AwfulRedux.Mobile.ViewModels;
using Xamarin.Forms;

namespace AwfulRedux.Mobile.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly LoginPageViewModel _viewModel;

        public LoginPage()
        {
            InitializeComponent();
            _viewModel = (LoginPageViewModel) BindingContext;
        }

        async void OnLoginClicked(object sender, EventArgs e)
        {
            if (_viewModel.CanLogin())
            {
                var result = await _viewModel.Login();
                if (!result)
                {
                    await DisplayAlert("Error", _viewModel.ValidationErrors, "OK");
                }
                else
                {
                   await _viewModel.NavigateToSettings();
                }
            }
            else
            {
                await DisplayAlert("Error", _viewModel.ValidationErrors, "OK");
            }
        }
    }
}
