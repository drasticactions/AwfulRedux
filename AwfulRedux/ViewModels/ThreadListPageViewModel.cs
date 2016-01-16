using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
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

        private bool _isLoading = default(bool);
        private Thread _selected = default(Thread);

        public Thread Selected
        {
            get { return _selected; }
            set
            {
                Set(ref _selected, value);
            }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                Set(ref _isLoading, value);
            }
        }

        public override void OnNavigatedTo(object parameter, NavigationMode mode,
            IDictionary<string, object> state)
        {
            if (state.ContainsKey(nameof(Selected)))
            {
                if (Selected == null)
                {
                    Selected = JsonConvert.DeserializeObject<Thread>(state[nameof(Selected)]?.ToString());
                    state.Clear();
                }
            }

            if (mode == NavigationMode.Forward | mode == NavigationMode.Back)
            {
                return;
            }

            var forum = JsonConvert.DeserializeObject<Forum>((string)parameter);
            if (forum == null) return;
            Forum = forum;
            ForumPageScrollingCollection = new PageScrollingCollection(Forum, 1);
            ForumPageScrollingCollection.CheckIsPaywallEvent += ForumPageScrollingCollection_CheckIsPaywallEvent;
        }

        public override Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            if (suspending)
            {
                state[nameof(Selected)] = JsonConvert.SerializeObject(Selected);
            }
            return Task.CompletedTask;
        }

        public void Refresh()
        {
            ForumPageScrollingCollection.Clear();
            ForumPageScrollingCollection.HasMoreItems = true;
        }

        private void ForumPageScrollingCollection_CheckIsPaywallEvent(object sender, PageScrollingCollection.IsPaywallArgs e)
        {
            if (!e.IsPaywall) return;
            NavigationService.Navigate(typeof (PaywallPage));
            var length = NavigationService.Frame.BackStack.Count;
            NavigationService.Frame.BackStack.RemoveAt(length - 1);
        }

        public void ReplyToThread(Thread thread)
        {
            NavigationService.Navigate(typeof(ReplyPage), JsonConvert.SerializeObject(thread));
        }

        public void CreateThread()
        {
            if (Views.Shell.Instance.ViewModel.IsLoggedIn)
            {
                NavigationService.Navigate(typeof(NewThreadPage), JsonConvert.SerializeObject(Forum));
            }
            else
            {
                // TODO: Figure out what to show here?
            }
        }
    }
}
