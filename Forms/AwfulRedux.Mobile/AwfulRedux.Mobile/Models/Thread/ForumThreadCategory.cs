using System.Collections.ObjectModel;

namespace AwfulRedux.Mobile.Models.Thread
{
	public class ForumThreadCategory : ObservableCollection<AwfulRedux.UI.Models.Forums.Forum>
	{
		public string Name { get; set; }

		public string Location { get; set; }

		public int Order { get; set; }
	}
}

