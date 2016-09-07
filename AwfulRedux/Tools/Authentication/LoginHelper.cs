using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AwfulForumsLibrary.Managers;
using AwfulRedux.Database;
using AwfulRedux.Tools.Database;


namespace AwfulRedux.Tools.Authentication
{
    public class LoginHelper
    {
        private static readonly AuthenticatedUserDatabase _udb = new AuthenticatedUserDatabase(DatabaseWinRTHelpers.GetWinRTDatabasePath("ForumsRedux.db"));

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
