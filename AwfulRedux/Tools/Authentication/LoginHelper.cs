using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AwfulRedux.Core.Managers;
using AwfulRedux.Database;
using AwfulRedux.Tools.Database;
using SQLite.Net.Platform.WinRT;

namespace AwfulRedux.Tools.Authentication
{
    public class LoginHelper
    {
        private static readonly AuthenticatedUserDatabase _udb = new AuthenticatedUserDatabase(new SQLitePlatformWinRT(), DatabaseWinRTHelpers.GetWinRTDatabasePath("ForumsRedux.db"));

        public static async Task<CookieContainer> LoginDefaultUser()
        {
            var defaultUsers = await _udb.GetAuthUsers();
            if (!defaultUsers.Any())
            {
                return null;
            }
            var defaultUser = defaultUsers.First();
            return await CookieManager.LoadCookie(defaultUser.Id + ".txt");
        } 
    }
}
