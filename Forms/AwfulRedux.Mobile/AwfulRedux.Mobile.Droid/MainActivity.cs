using System;
using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace AwfulRedux.Mobile.Droid
{
    [Activity(Label = "AwfulRedux.Mobile", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.tabs;
            ToolbarResource = Resource.Layout.toolbar;
            var x = typeof(Xamarin.Forms.Themes.LightThemeResources);
            x = typeof(Xamarin.Forms.Themes.LightThemeResources);
            x = typeof(Xamarin.Forms.Themes.Android.UnderlineEffect);
            base.OnCreate(bundle);
            UserDialogs.Init(this);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }
    }
}

