using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AwfulRedux.Mobile.Droid;
using AwfulRedux.Mobile.Tools;
using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinAndroid;
using Xamarin.Forms;

[assembly: Dependency(typeof(Sqlite_Android))]
namespace AwfulRedux.Mobile.Droid
{
    public class Sqlite_Android : ISQLite
    {
        public ISQLitePlatform GetPlatform()
        {
            return new SQLitePlatformAndroid();
        }

        public string GetPath(string file)
        {
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            return Path.Combine(documentsPath, file);
        }
    }
}