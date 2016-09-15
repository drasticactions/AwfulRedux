using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using AwfulForumsLibrary.Managers;
using AwfulRedux.UI.Models.Smilies;
using Newtonsoft.Json;

namespace SmiliesParser
{
	class MainClass
	{
		public static void Main (string [] args)
		{
			MainAsync().GetAwaiter().GetResult();
		}

		static async Task MainAsync()
		{
			var authManager = new AuthenticationManager();

			// Set the username / password of the user.
			var username = "";
			var password = "";

			var authResult = await authManager.AuthenticateAsync(username, password);

			// Once authed, we can setup the web manager and make authed calls.
			var webManager = new WebManager(authResult.AuthenticationCookie);

			var smileManager = new SmileManager(webManager);
			var result = await smileManager.GetSmileList();
			var list = JsonConvert.DeserializeObject<List<SmileCategory>>(result.ResultJson);

            File.WriteAllText("smileList.json", JsonConvert.SerializeObject(list, Formatting.Indented));
		}
	}
}
