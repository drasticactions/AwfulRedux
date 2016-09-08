using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Net;
using AwfulRedux.UI.Models.Users;
using AwfulRedux.Database;
using AwfulForumsLibrary.Managers;
using AwfulForumsLibrary.Tools;

namespace AwfulRedux_iOS
{
	public class AuthDataSource
	{
		static readonly AuthenticatedUserDatabase _udb = new AuthenticatedUserDatabase(DatabaseHelpers.GetiOSDatabasePath("ForumsRedux.db"));

		public static async Task<bool> AreAuthUsers() 
		{
			var defaultUsers = await _udb.GetAuthUsers();
			return defaultUsers.Any();
		}

		public static async Task<User> GetAuthUser() 
		{
			var defaultUsers = await _udb.GetAuthUsers();
			return defaultUsers.FirstOrDefault();
		}

		public static async Task<WebManager> GetWebManager() {
			var defaultUsers = await _udb.GetAuthUsers();
			if (!defaultUsers.Any())
			{
				return new WebManager();
			}
			var defaultUser = defaultUsers.First();
			var cookieManager = new CookieManager();
			var cookie = await cookieManager.LoadCookie(defaultUser.Id + ".txt");
			return new WebManager(cookie);
		}

		public async static Task<bool> SaveUser(User user, CookieContainer cookies) 
		{
			try {
				await _udb.AddOrUpdateUser(user);
				var cookieManager = new CookieManager();
				return await cookieManager.SaveCookie(user.Id + ".txt", cookies, new Uri(EndPoints.CookieDomainUrl));
			}
			catch {
				return false;
			}
		}
	}
}
