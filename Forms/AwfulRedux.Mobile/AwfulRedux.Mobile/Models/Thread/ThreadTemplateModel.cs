using System.Collections.Generic;
using AwfulRedux.UI.Models.Posts;

namespace AwfulRedux.Mobile.Models.Thread
{
	public class ThreadTemplateModel
	{
		public UI.Models.Threads.Thread ForumThread { get; set; }

		public List<Post> Posts { get; set; }

		public bool IsLoggedIn { get; set; }

		public bool IsDarkThemeSet { get; set; }
	}
}

