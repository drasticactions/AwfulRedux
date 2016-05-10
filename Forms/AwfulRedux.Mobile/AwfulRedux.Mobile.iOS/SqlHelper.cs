using System;
using System.IO;
using AwfulRedux.Mobile.iOS;
using AwfulRedux.Mobile.Tools;
using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinIOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(Sqlite_iOS))]
namespace AwfulRedux.Mobile.iOS
{
    public class Sqlite_iOS : ISQLite
    {
        public ISQLitePlatform GetPlatform()
        {
            return new SQLitePlatformIOS();
        }

        public string GetPath(string file)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            return Path.Combine(documentsPath, file);
        }
    }
}
