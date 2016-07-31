using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

using AwfulRedux.Controls;
using AwfulForumsLibrary.Managers;
using AwfulRedux.Tools.ScrollingCollection;
using AwfulRedux.Tools.Web;
using AwfulRedux.UI;
using AwfulRedux.UI.Models.Forums;
using AwfulRedux.UI.Models.Posts;
using AwfulRedux.UI.Models.Threads;
using AwfulRedux.Views;
using Kimono.Controls;
using Newtonsoft.Json;
using RefreshableListView;
using Template10.Mvvm;

namespace AwfulRedux.ViewModels
{
    public class ThreadListPageViewModel : ForumThreadListBaseViewModel
    {
        private PageScrollingCollection _ForumPageScrollingCollection = default(PageScrollingCollection);
        public PageScrollingCollection ForumPageScrollingCollection
        {
            get { return _ForumPageScrollingCollection; }
            set
            {
                Set(ref _ForumPageScrollingCollection, value);
            }
        }
        private Forum _forum = default(Forum);
        public Forum Forum
        {
            get { return _forum; }
            set
            {
                Set(ref _forum, value);
            }
        }

        public async void PullToRefresh_ListView(object sender, EventArgs e)
        {
            Refresh();
        }

        private bool _threadLoaded = default(bool);
        public bool ThreadLoaded
        {
            get { return _threadLoaded; }
            set
            {
                Set(ref _threadLoaded, value);
            }
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            if (WebManager == null)
            {
                await LoginUser();
            }
            Template10.Common.BootStrapper.Current.NavigationService.FrameFacade.BackRequested += MasterDetailViewControl.NavigationManager_BackRequested;
            if (Forum != null && (mode == NavigationMode.Forward | mode == NavigationMode.Back))
            {
                return;
            }

            if (suspensionState.ContainsKey(nameof(Forum)))
            {
                if (Forum == null)
                {
                    Forum = JsonConvert.DeserializeObject<Forum>(suspensionState[nameof(Forum)]?.ToString());
                }
            }
            else
            {
                var forum = JsonConvert.DeserializeObject<Forum>((string)parameter);
                if (forum == null) return;
                Forum = forum;
            }

            ForumPageScrollingCollection = new PageScrollingCollection(Forum, 1);
            ForumPageScrollingCollection.CheckIsPaywallEvent += ForumPageScrollingCollection_CheckIsPaywallEvent;

            if (suspensionState.ContainsKey(nameof(Selected)))
            {
                if (Selected == null)
                {
                    Selected = JsonConvert.DeserializeObject<Thread>(suspensionState[nameof(Selected)]?.ToString());
                    await ThreadView.LoadThread(Selected, true);
                }
            }

            suspensionState.Clear();
        }

        public override Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            Template10.Common.BootStrapper.Current.NavigationService.FrameFacade.BackRequested -= MasterDetailViewControl.NavigationManager_BackRequested;
            if (suspending)
            {
                if (Selected != null)
                {
                    var newThread = Selected.Clone();
                    newThread.Html = null;
                    newThread.Posts = null;
                    state[nameof(Selected)] = JsonConvert.SerializeObject(newThread);
                }
                state[nameof(Forum)] = JsonConvert.SerializeObject(Forum);
            }
            return Task.CompletedTask;
        }

        public void Refresh()
        {
            ForumPageScrollingCollection = new PageScrollingCollection(Forum, 1);
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
