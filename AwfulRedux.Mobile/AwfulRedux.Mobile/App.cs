using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AwfulRedux.Mobile.Views;
using Xamarin.Forms;

namespace AwfulRedux.Mobile
{
    public class App : Application
    {
        public App()
        {
            // The root page of your application
            MainPage = GetMainPage();
        }

        public Page GetMainPage()
        {
            var tp = new TabbedPage();
            tp.Children.Add(new NavigationPage(new MainForumsPage()));
            tp.Children.Add(new ContentPage { BackgroundColor = Color.Blue, Title = "Page 2" });
            tp.Children.Add(new ContentPage { BackgroundColor = Color.Green, Title = "Page 3" });
            return tp;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
