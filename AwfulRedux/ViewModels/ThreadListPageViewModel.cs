using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using AwfulRedux.Core.Managers;
using AwfulRedux.Tools.ScrollingCollection;
using AwfulRedux.Tools.Web;
using AwfulRedux.UI;
using AwfulRedux.UI.Models.Forums;
using AwfulRedux.UI.Models.Posts;
using AwfulRedux.UI.Models.Threads;
using AwfulRedux.Views;
using Newtonsoft.Json;
using Template10.Mvvm;

namespace AwfulRedux.ViewModels
{
    public class ThreadListPageViewModel : ViewModelBase
    {

        public PageScrollingCollection ForumPageScrollingCollection { get; set; }
        
        public Forum Forum { get; set; }

        private bool _isPaywall = default(bool);
        private Thread _selected = default(Thread);

        public Thread Selected
        {
            get { return _selected; }
            set
            {
                Set(ref _selected, value);
            }
        }

        public bool IsPaywall
        {
            get { return _isPaywall; }
            set
            {
                Set(ref _isPaywall, value);
            }
        }

        public override void OnNavigatedTo(object parameter, NavigationMode mode,
            IDictionary<string, object> state)
        {
            var forum = parameter as Forum;
            if (forum == null) return;
            Forum = forum;
            ForumPageScrollingCollection = new PageScrollingCollection(Forum, 1);
            ForumPageScrollingCollection.CheckIsPaywallEvent += ForumPageScrollingCollection_CheckIsPaywallEvent;
        }

        private void ForumPageScrollingCollection_CheckIsPaywallEvent(object sender, PageScrollingCollection.IsPaywallArgs e)
        {
            if (!e.IsPaywall) return;
            NavigationService.Navigate(typeof (PaywallPage));
            var length = NavigationService.Frame.BackStack.Count;
            NavigationService.Frame.BackStack.RemoveAt(length - 1);
        }

        public async Task LoadThread(Thread thread)
        {
            var tempManager = new PostManager(Views.Shell.Instance.WebManager);
            var result = await tempManager.GetThreadPostsAsync(thread.Location, 0);
            var postresult = JsonConvert.DeserializeObject<List<Post>>(result.ResultJson);
            Selected.Posts = postresult;
            Selected.Html = await HtmlFormater.FormatThreadHtml(Selected, postresult, PlatformIdentifier.Windows8);
        }
    }
}
