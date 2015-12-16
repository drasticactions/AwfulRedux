using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using AwfulRedux.Core.Managers;
using AwfulRedux.Tools.ScrollingCollection;
using AwfulRedux.UI.Models.Forums;
using AwfulRedux.UI.Models.Posts;
using AwfulRedux.UI.Models.Threads;
using Newtonsoft.Json;
using Template10.Mvvm;

namespace AwfulRedux.ViewModels
{
    public class ThreadListPageViewModel : ViewModelBase
    {
        public PageScrollingCollection ForumPageScrollingCollection { get; set; }
        
        public Forum Forum { get; set; }

        private Thread _selected = default(Thread);

        public Thread Selected
        {
            get { return _selected; }
            set
            {
                Set(ref _selected, value);
            }
        }

        public override void OnNavigatedTo(object parameter, NavigationMode mode,
            IDictionary<string, object> state)
        {
            var forum = parameter as Forum;
            if (forum == null) return;
            Forum = forum;
            ForumPageScrollingCollection = new PageScrollingCollection(Forum, 1);
        }

        public async Task LoadThread(Thread thread)
        {
            var tempManager = new PostManager(Views.Shell.Instance.WebManager);
            var result = await tempManager.GetThreadPostsAsync(thread.Location, 0);
            var postresult = JsonConvert.DeserializeObject<List<Post>>(result.ResultJson);
            thread.Posts = postresult;
            Selected = thread;
        }
    }
}
